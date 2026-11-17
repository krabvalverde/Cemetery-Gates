"""Sintetiza os efeitos sonoros do jogo (sem samples externos) em Assets/Audio/SFX.

Uso: python gerar_sons.py   (precisa só de numpy)
Cada som é feito de ruído filtrado, osciladores e ressonadores. Mexa nos parâmetros
de cada função e rode de novo; o Unity reimporta os .wav sozinho.
"""
import os
import wave

import numpy as np

SR = 44100
OUT = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "SFX")
rng = np.random.default_rng(7)


# ---------- blocos básicos ----------

def n(sec):
    return int(sec * SR)


def t(sec):
    return np.arange(n(sec)) / SR


def noise(sec):
    return rng.uniform(-1, 1, n(sec))


def decay(sec, tau, start=0.0):
    x = t(sec)
    return np.where(x >= start, np.exp(-(x - start) / tau), 0.0)


def ramp_env(sec, points):
    """points = [(tempo, ganho), ...] interpolado linearmente."""
    times, gains = zip(*points)
    return np.interp(t(sec), times, gains)


def smooth_noise(sec, rate, amount=1.0):
    """Ruído lento (jitter), 'rate' valores por segundo."""
    k = max(2, int(sec * rate) + 2)
    return np.interp(t(sec), np.linspace(0, sec, k), rng.uniform(-1, 1, k)) * amount


def biquad(x, kind, f, q=0.707):
    f = np.broadcast_to(np.asarray(f, dtype=float), x.shape)
    w = 2 * np.pi * np.clip(f, 20, SR * 0.45) / SR
    cw, sw = np.cos(w), np.sin(w)
    alpha = sw / (2 * q)
    if kind == "lp":
        b0, b1, b2 = (1 - cw) / 2, 1 - cw, (1 - cw) / 2
    elif kind == "hp":
        b0, b1, b2 = (1 + cw) / 2, -(1 + cw), (1 + cw) / 2
    else:  # bp, pico de 0 dB
        b0, b1, b2 = alpha, np.zeros_like(alpha), -alpha
    a0, a1, a2 = 1 + alpha, -2 * cw, 1 - alpha
    b0, b1, b2, a1, a2 = b0 / a0, b1 / a0, b2 / a0, a1 / a0, a2 / a0
    y = np.zeros_like(x)
    x1 = x2 = y1 = y2 = 0.0
    for i in range(len(x)):
        xi = x[i]
        yi = b0[i] * xi + b1[i] * x1 + b2[i] * x2 - a1[i] * y1 - a2[i] * y2
        x2, x1, y2, y1 = x1, xi, y1, yi
        y[i] = yi
    return y


def sine_sweep(sec, f0, f1, curve=3.0):
    x = t(sec)
    f = f1 + (f0 - f1) * np.exp(-x * curve / sec * 3)
    return np.sin(2 * np.pi * np.cumsum(f) / SR)


def pad(x, sec):
    out = np.zeros(n(sec))
    out[:min(len(x), len(out))] = x[:len(out)]
    return out


def place(dst, src, at, gain=1.0):
    i = n(at)
    end = min(len(dst), i + len(src))
    dst[i:end] += src[:end - i] * gain
    return dst


def reverb(x, tail=0.6, mix=0.2, tone=2500):
    ir = noise(tail) * decay(tail, tail / 5)
    ir = biquad(ir, "lp", tone)
    size = len(x) + len(ir)
    wet = np.fft.irfft(np.fft.rfft(x, size) * np.fft.rfft(ir, size), size)[:len(x)]
    wet /= np.max(np.abs(wet)) + 1e-9
    return x * (1 - mix) + wet * mix * np.max(np.abs(x))


def voice(sec, f0, formants, breath=0.15, fry=0.0, jitter=0.03):
    """Voz rouca: fonte serrilhada com jitter passando por filtros de formante.
    f0 e cada formante podem ser arrays (variação no tempo)."""
    f0 = np.broadcast_to(np.asarray(f0, dtype=float), (n(sec),)) * (1 + smooth_noise(sec, 25, jitter))
    phase = np.cumsum(f0) / SR
    src = 1 - 2 * (phase % 1.0)
    if fry:
        src *= 1 - fry + fry * (np.sin(2 * np.pi * np.cumsum(28 + smooth_noise(sec, 10, 8)) / SR) > 0.3)
    src = src + noise(sec) * breath
    out = np.zeros(n(sec))
    for f, gain, bw in formants:
        out += biquad(src, "bp", f, q=np.asarray(f) / bw) * gain
    return norm(out, 1.0)


def resonators(excite, modes):
    return sum(biquad(excite, "bp", f, q) * g for f, q, g in modes)


def norm(x, peak=0.9):
    x = x - np.mean(x)
    return x / (np.max(np.abs(x)) + 1e-9) * peak


def fade(x, ms=8):
    k = min(len(x), int(SR * ms / 1000))
    x[-k:] *= np.linspace(1, 0, k)
    return x


def save(name, x, peak=0.9):
    os.makedirs(OUT, exist_ok=True)
    x = fade(norm(np.tanh(x * 1.2), peak))
    data = (x * 32767).astype(np.int16)
    with wave.open(os.path.join(OUT, name + ".wav"), "wb") as w:
        w.setnchannels(1)
        w.setsampwidth(2)
        w.setframerate(SR)
        w.writeframes(data.tobytes())
    print(f"{name}.wav  {len(x) / SR:.2f}s")


# ---------- detetive ----------

def tiro():
    """Revólver ao ar livre: pulso de pressão + estouro dos gases, saturado como numa gravação,
    reflexões próximas (chão, lápides, árvores) e o 'trovão' rolando ao longe."""
    L = 2.2
    tt = t(L)

    # pulso da boca do cano (onda de Friedlander): sobe na hora, cai e passa para o negativo
    T = 0.0025
    blast = np.where(tt < 0.03, (1 - tt / T) * np.exp(-4 * tt / T), 0.0)
    blast = biquad(blast, "lp", 9000)

    # gases saindo: ruído largo, muito curto, mais agudo no começo
    gas = biquad(noise(L), "bp", np.interp(tt, [0, 0.02], [3200, 1200]), 0.7) * decay(L, 0.007)
    # corpo do estampido: ruído grave (sem tom de "bumbo"), cai em ~60 ms
    body = biquad(biquad(noise(L), "lp", 900), "hp", 70) * decay(L, 0.045)
    x = blast * 1.4 + gas * 0.9 + body * 1.1

    # gravação de arma estoura: saturação pesada dá o "soco"
    x = np.tanh(norm(x, 1.0) * 5.0)

    # reflexões próximas: cópias abafadas e espalhadas em 15–140 ms
    dry = x.copy()
    for _ in range(9):
        at = rng.uniform(0.015, 0.14)
        place(x, biquad(dry, "lp", rng.uniform(1200, 3500)), at, rng.uniform(0.08, 0.25) * np.exp(-at / 0.12))

    # trovão: cauda difusa que escurece e oscila enquanto some
    tail = biquad(noise(L), "lp", np.interp(tt, [0, 0.3, 1.2, L], [2500, 1200, 500, 300]))
    tail *= ramp_env(L, [(0, 0), (0.04, 1), (L, 0)]) ** 2.5 * (1 + 0.35 * smooth_noise(L, 7))
    x += norm(tail, 1.0) * 0.16
    # 3 ms de silêncio antes: o pulso sobe de zero (sem estalo no início do arquivo); corta o fim já mudo
    return np.concatenate((np.zeros(n(0.003)), x[:n(1.7)]))


def click(sec, f, tau=0.004, ping=0.0, ping_f=2500):
    x = biquad(noise(sec), "bp", f, 2.0) * decay(sec, tau)
    if ping:
        x += np.sin(2 * np.pi * ping_f * t(sec)) * decay(sec, 0.03) * ping
    return x


def tiro_vazio():
    x = click(0.25, 3800, 0.003, ping=0.4, ping_f=2300)
    return place(x, click(0.1, 2000, 0.004), 0.05, 0.4)


def recarga():
    x = np.zeros(n(1.3))
    place(x, click(0.2, 2500, 0.006, 0.5, 1800), 0.0)           # abre o tambor
    for at, f in [(0.28, 5200), (0.43, 4800), (0.58, 5000)]:     # balas entrando
        place(x, click(0.15, f, 0.003, 0.6, f * 0.8), at, 0.7)
    for i in range(7):                                           # gira o tambor (catraca)
        place(x, click(0.05, 3200, 0.002), 0.8 + i * 0.03, 0.5)
    place(x, click(0.25, 1800, 0.008, 0.7, 1400), 1.05, 1.0)    # fecha
    return x


def whoosh(sec, f_lo, f_hi, peak_at=0.5):
    env = ramp_env(sec, [(0, 0), (sec * peak_at, 1), (sec, 0)]) ** 2
    f = np.interp(t(sec), [0, sec * peak_at, sec], [f_lo, f_hi, f_lo * 1.3])
    return biquad(noise(sec), "bp", f, 1.5) * env


def soco_ar():
    return whoosh(0.3, 350, 1400, 0.45)


def impacto(sec=0.35, f=95, weight=1.0):
    thud = sine_sweep(sec, f, f * 0.5) * decay(sec, 0.06 * weight)
    mush = biquad(noise(sec), "lp", 700) * decay(sec, 0.03)
    slap = biquad(noise(sec), "hp", 2500) * decay(sec, 0.004)
    return thud + mush * 0.8 + slap * 0.3


def soco_acerto():
    return impacto(0.35, 95)


def contato(L, weight):
    """Sapato batendo na terra: baque surdo (ruído grave, sem tom) + toque seco do salto/sola."""
    thud = biquad(biquad(noise(L), "lp", 260), "hp", 45) * ramp_env(L, [(0, 0), (0.0015, 1), (L, 0)]) * decay(L, 0.02)
    knock = biquad(noise(L), "bp", rng.uniform(700, 1100), 1.3) * decay(L, 0.005)
    return norm(thud, 1.0) * weight + norm(knock, 1.0) * 0.6 * weight


def cascalho(L, start, length, count, level):
    """Grãos de terra/cascalho estalando enquanto o peso do pé assenta."""
    x = np.zeros(n(L))
    for _ in range(count):
        at = start + rng.gamma(1.6, length / 4)
        if at > L - 0.005:
            continue
        g = biquad(noise(0.004), "bp", rng.uniform(1300, 5500), rng.uniform(2, 4)) * decay(0.004, rng.uniform(0.0003, 0.0009))
        place(x, norm(g, 1.0), at, level * rng.uniform(0.3, 1.0) * np.exp(-(at - start) / length))
    return x


def passo(variant):
    """Passo de sapato em terra batida com cascalho: calcanhar, sola arrastando e ponta do pé."""
    L = 0.3
    heel, toe = 0.004, 0.004 + rng.uniform(0.07, 0.11)
    x = np.zeros(n(L))
    place(x, contato(0.12, 1.0), heel)
    place(x, contato(0.10, rng.uniform(0.4, 0.6)), toe)
    x += cascalho(L, heel, 0.07, rng.integers(28, 42), 0.75)
    x += cascalho(L, toe, 0.05, rng.integers(12, 20), 0.45)
    scuff = biquad(noise(L), "bp", rng.uniform(1500, 2400), 0.9)
    scuff *= ramp_env(L, [(0, 0), (heel + 0.01, 0), (heel + 0.04, 1), (toe + 0.02, 0.6), (toe + 0.08, 0), (L, 0)])
    return x + norm(scuff, 1.0) * rng.uniform(0.05, 0.1)


def detetive_dano():
    L = 0.45
    f0 = np.interp(t(L), [0, L], [125, 92])
    v = voice(L, f0, [(640, 1.0, 90), (1190, 0.5, 110), (2390, 0.25, 160)], breath=0.3, jitter=0.04)
    v *= ramp_env(L, [(0, 0), (0.02, 1), (0.18, 0.6), (L, 0)])
    return impacto(L, 90) * 0.8 + v * 0.9


def detetive_morte():
    L = 1.6
    f0 = np.interp(t(L), [0, 0.8, L], [130, 85, 70])
    v = voice(L, f0, [(750, 1.0, 100), (1100, 0.5, 120), (2500, 0.2, 170)], breath=0.35, fry=0.4)
    v *= ramp_env(L, [(0, 0), (0.05, 1), (0.6, 0.7), (0.95, 0)])
    return place(v, impacto(0.5, 70, 1.6), 1.0, 1.1)


# ---------- zumbi ----------
# Voz feita como uma voz de verdade: pulsos das pregas vocais (irregulares, rangidos) + ar + gorgolejo,
# passando por um trato vocal em cascata cujas vogais mudam no tempo. Garganta "grande e estragada":
# formantes um pouco mais graves e largos que os de um homem.

VOGAIS = {  # F1, F2, F3 (Hz) de voz masculina
    "uh": (640, 1190, 2390), "oh": (500, 900, 2400), "ah": (750, 1100, 2450),
    "oo": (350, 800, 2300), "eh": (530, 1700, 2500), "mm": (260, 1000, 2200),
}


def glotal(L, f0_points, jitter, shimmer, subharm, env_points):
    """Trem de pulsos glotais (Rosenberg), um período por vez, com jitter/shimmer por pulso.
    Devolve a derivada do fluxo (o que excita o trato) e o fluxo (para modular o ar)."""
    N = n(L)
    f0 = np.interp(t(L), *zip(*f0_points))
    env = np.interp(t(L), *zip(*env_points))
    dflow, flow = np.zeros(N + 4000), np.zeros(N + 4000)
    pos, k = 0, 0
    while pos < N:
        period = int(np.clip(SR / f0[pos] * (1 + jitter * rng.normal()), SR / 400, SR / 18))
        amp = env[pos] * max(0.0, 1 + shimmer * rng.normal()) * (1 - subharm if k % 2 else 1)
        tp, tn = int(period * 0.42), max(2, int(period * 0.16))
        pulse = np.zeros(period)
        pulse[:tp] = 0.5 * (1 - np.cos(np.pi * np.arange(tp) / tp))
        pulse[tp:tp + tn] = np.cos(0.5 * np.pi * np.arange(tn) / tn)
        flow[pos:pos + period] += pulse * amp
        dflow[pos:pos + period] += np.diff(pulse, prepend=0) * amp * period / 12
        pos += period
        k += 1
    return dflow[:N], flow[:N]


def ressonador(x, f, bw):
    """Ressonador de 2 polos (Klatt) com ganho unitário em DC; f e bw podem variar no tempo."""
    f = np.broadcast_to(np.asarray(f, dtype=float), x.shape)
    bw = np.broadcast_to(np.asarray(bw, dtype=float), x.shape)
    C = -np.exp(-2 * np.pi * bw / SR)
    B = 2 * np.exp(-np.pi * bw / SR) * np.cos(2 * np.pi * f / SR)
    A = 1 - B - C
    y = np.zeros_like(x)
    y1 = y2 = 0.0
    for i in range(len(x)):
        yi = A[i] * x[i] + B[i] * y1 + C[i] * y2
        y2, y1 = y1, yi
        y[i] = yi
    return y


def gorgolejo(L, rate, level):
    """Bolhas na garganta: estalos úmidos curtos, graves e afinados ao acaso."""
    x = np.zeros(n(L))
    for _ in range(int(L * rate)):
        d = rng.uniform(0.015, 0.04)
        b = np.sin(2 * np.pi * np.cumsum(np.linspace(rng.uniform(250, 500), rng.uniform(120, 250), n(d))) / SR)
        place(x, b * decay(d, d / 3), rng.uniform(0, L - d), rng.uniform(0.3, 1.0) * level)
    return x


def zumbi_voz(L, f0, vogais, env, jitter=0.12, shimmer=0.25, subharm=0.3, breath=0.35, gurgle=0.0,
              drive=1.5, escala=0.88, rasp=0.08):
    """f0/vogais/env = [(tempo, valor), ...]. Vogais pela chave de VOGAIS."""
    dflow, flow = glotal(L, f0, jitter, shimmer, subharm, env)
    ar = biquad(noise(L), "hp", 900) * (0.25 * np.interp(t(L), *zip(*env)) + flow)
    src = norm(dflow, 1.0) + norm(ar, 1.0) * breath
    if gurgle:
        src += gorgolejo(L, 9, gurgle)
    times = [p[0] for p in vogais]
    out = src
    for j, bw in enumerate((130, 170, 260)):
        fj = np.interp(t(L), times, [VOGAIS[v][j] * escala for _, v in vogais])
        out = ressonador(out, fj, bw)
    out = ressonador(ressonador(out, 3300 * escala, 350), 3800 * escala, 450)
    out = biquad(out, "hp", 60)
    # rouquidão: ar raspando na garganta, pulsando junto com as pregas vocais
    raspa = biquad(noise(L), "bp", 2400, 0.7) * biquad(flow, "lp", 300)
    out = norm(out, 1.0) + norm(raspa, 1.0) * rasp
    return np.tanh(norm(out, 1.0) * drive)


def carne(L, weight=1.0):
    """Golpe na carne: baque grave sem tom + estalo úmido."""
    thump = biquad(biquad(noise(L), "lp", 220), "hp", 40) * decay(L, 0.035 * weight)
    squelch = biquad(noise(L), "bp", rng.uniform(500, 800), 1.2) * decay(L, 0.05) * (1 + smooth_noise(L, 80, 0.8))
    return norm(thump, 1.0) * weight + norm(squelch, 1.0) * 0.45


def corpo_caindo(L):
    """Corpo inteiro batendo na terra: baque pesado, cascalho espalhando e os braços logo depois."""
    x = contato(L, 1.6) + norm(biquad(noise(L), "lp", 120) * decay(L, 0.08), 1.0) * 1.2
    x += cascalho(L, 0.0, 0.12, 70, 0.6)
    place(x, contato(0.15, 0.5), 0.13)
    place(x, contato(0.15, 0.35), 0.21)
    return x


def gemido(variant, L):
    rotas = [  # vogais ao longo do gemido: abre a boca e fecha no fim
        [(0, "mm"), (0.25, "uh"), (0.6, "oh"), (1.0, "mm")],
        [(0, "uh"), (0.35, "ah"), (0.7, "oh"), (1.0, "oo")],
        [(0, "oo"), (0.3, "uh"), (0.75, "uh"), (1.0, "mm")],
    ][variant]
    base = (58, 66, 52)[variant]
    f0 = [(0, base * 0.9), (L * 0.35, base * 1.12), (L * 0.75, base), (L, base * 0.8)]
    env = [(0, 0), (L * 0.25, 1), (L * 0.7, 0.75), (L, 0)]
    v = zumbi_voz(L, f0, [(L * a, k) for a, k in rotas], env, gurgle=0.25 if variant == 1 else 0.1)
    if variant == 2:  # puxa o ar antes, rouco
        inhale = biquad(noise(0.45), "bp", 1400, 1.2) * ramp_env(0.45, [(0, 0), (0.35, 1), (0.45, 0)])
        v = np.concatenate((norm(inhale, 0.35), v))
    return reverb(v, 0.8, 0.15)


def zumbi_rosnado():
    L = 1.2
    f0 = [(0, 85), (0.15, 110), (0.6, 95), (L, 70)]
    env = [(0, 0), (0.06, 1), (0.7, 0.9), (L, 0)]
    v = zumbi_voz(L, f0, [(0, "uh"), (0.12, "ah"), (0.8, "ah"), (L, "uh")], env,
                  jitter=0.2, shimmer=0.35, subharm=0.55, breath=0.6, gurgle=0.3, drive=3.0, rasp=0.9)
    v *= 1 + 0.4 * smooth_noise(L, 40)  # pregas falsas tremendo
    return reverb(v, 0.7, 0.12)


def zumbi_ataque():
    L = 0.85
    x = np.zeros(n(L))
    inhale = biquad(noise(0.22), "bp", 1700, 1.0) * ramp_env(0.22, [(0, 0), (0.18, 1), (0.22, 0)])
    place(x, norm(inhale, 0.4), 0.0)
    bark = zumbi_voz(0.45, [(0, 120), (0.1, 135), (0.45, 80)], [(0, "eh"), (0.08, "ah"), (0.45, "uh")],
                     [(0, 0), (0.03, 1), (0.25, 0.7), (0.45, 0)], jitter=0.2, subharm=0.5, breath=0.7, drive=3.0, rasp=0.45)
    place(x, bark, 0.2)
    return place(x, whoosh(0.3, 250, 900, 0.5), 0.3, 0.9)  # braços passando; golpe em ~0,45 s


def zumbi_dor():
    L = 0.55
    grunt = zumbi_voz(0.4, [(0, 150), (0.05, 125), (0.4, 75)], [(0, "uh"), (0.15, "eh"), (0.4, "mm")],
                      [(0, 0), (0.015, 1), (0.12, 0.6), (0.4, 0)], jitter=0.15, subharm=0.4, breath=0.5, drive=2.2, rasp=0.3)
    x = carne(L) * 0.9
    return place(x, grunt, 0.02, 0.9)


def zumbi_morte():
    L = 2.1
    f0 = [(0, 95), (0.3, 80), (1.0, 50), (1.2, 40)]
    env = [(0, 0), (0.05, 1), (0.8, 0.6), (1.15, 0.25), (1.25, 0)]
    v = zumbi_voz(L, f0, [(0, "ah"), (0.5, "uh"), (0.95, "oh"), (1.2, "mm")], env,
                  jitter=0.15, shimmer=0.35, subharm=0.45, breath=0.45, gurgle=0.45, drive=2.0, rasp=0.2)
    v = place(v, corpo_caindo(0.6), 1.22, 1.0)  # anim Death toca o chão em 1,25 s
    return reverb(v, 0.8, 0.12)


# ---------- cenário ----------

def rangido(L, rate, modes, rough=0.3):
    """Atrito de dobradiça: trem de impulsos irregulares excitando ressonâncias."""
    rate = np.broadcast_to(np.asarray(rate, dtype=float), (n(L),)) * (1 + smooth_noise(L, 12, rough))
    phase = np.cumsum(rate) / SR
    pulses = (np.diff(np.floor(phase), prepend=0) > 0).astype(float)
    pulses *= rng.uniform(0.5, 1.0, len(pulses))
    return norm(resonators(pulses, modes), 1.0)


def metal_batida(sec, modes):
    x = np.zeros(n(sec))
    tt = t(sec)
    for f, tau, g in modes:
        x += np.sin(2 * np.pi * f * tt) * np.exp(-tt / tau) * g
    return x + click(sec, 3000, 0.003) * 0.5


def porta_abrindo():
    L = 1.5
    rate = np.interp(t(L), [0, 0.4, 0.9, 1.2, L], [35, 110, 70, 50, 50])
    c = rangido(L, rate, [(260, 8, 1.0), (720, 10, 0.6), (1500, 12, 0.3)], rough=0.4)
    c *= ramp_env(L, [(0, 0), (0.05, 1), (1.1, 0.7), (1.25, 0), (L, 0)])
    return reverb(place(c, impacto(0.3, 120, 0.5), 1.18, 0.4), 0.6, 0.2)


def destrancar():
    x = np.zeros(n(0.6))
    place(x, click(0.1, 2600, 0.004, 0.3, 1900), 0.0)
    place(x, click(0.1, 2200, 0.005, 0.3, 1600), 0.12)
    place(x, metal_batida(0.4, [(900, 0.06, 1.0), (2100, 0.04, 0.5)]), 0.25)
    return x


def trancado():
    x = np.zeros(n(0.6))
    for at in (0.0, 0.11, 0.2):
        place(x, metal_batida(0.3, [(700, 0.05, 1.0), (1800, 0.03, 0.4)]) + impacto(0.3, 110, 0.4) * 0.5, at, rng.uniform(0.6, 1.0))
    return x


def item_pegar():
    L = 0.35
    env = ramp_env(L, [(0, 0), (0.05, 1), (0.12, 0.4), (0.18, 0.9), (L, 0)])
    rustle = biquad(noise(L), "bp", 3500, 0.8) * env * (0.6 + 0.4 * (rng.uniform(0, 1, n(L)) > 0.7))
    return rustle * 0.6 + impacto(L, 160, 0.4) * 0.3


if __name__ == "__main__":
    save("tiro", tiro(), 0.95)
    save("tiro_vazio", tiro_vazio(), 0.6)
    save("recarga", recarga(), 0.6)
    save("soco_ar", soco_ar(), 0.5)
    save("soco_acerto", soco_acerto(), 0.8)
    for i in range(6):
        save(f"passo_{i + 1}", passo(i), 0.45)
    save("detetive_dano", detetive_dano(), 0.8)
    save("detetive_morte", detetive_morte(), 0.8)
    for i, L in enumerate((1.8, 2.3, 1.5)):
        save(f"zumbi_gemido_{i + 1}", gemido(i, L), 0.6)
    save("zumbi_rosnado", zumbi_rosnado(), 0.85)
    save("zumbi_ataque", zumbi_ataque(), 0.85)
    save("zumbi_dor", zumbi_dor(), 0.8)
    save("zumbi_morte", zumbi_morte(), 0.85)
    save("porta_abrindo", porta_abrindo(), 0.75)
    save("destrancar", destrancar(), 0.6)
    save("trancado", trancado(), 0.6)
    save("item_pegar", item_pegar(), 0.5)
