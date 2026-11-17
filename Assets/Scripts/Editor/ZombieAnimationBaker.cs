using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

// O FBX do zumbi só tem Idle. Este gerador cria Walk, Attack e Death posando o rig Mixamo por código
// (IK de 2 ossos nas pernas, braços apontados) e grava os ossos em clipes .anim editáveis.
// ponytail: animação procedural; trocar por clipes de verdade (Mixamo/Blender) se surgirem.
static class ZombieAnimationBaker
{
    const string PrefabPath = "Assets/Prefabs/Zombie.prefab";
    const string ControllerPath = "Assets/Animations/Zombie.controller";
    const float WalkSpeed = 1.2f;   // = Enemy.speed; o Enemy escala animator.speed pela velocidade real
    const float WalkPeriod = 1.0f;
    const float AttackLength = 1.2f; // golpe acerta em ~0,45 s (Enemy.attackHitDelay)
    const float DeathLength = 1.5f;
    const int Fps = 30;

    struct Pose
    {
        public Vector3 hips;           // deslocamento do quadril
        public float hipTwist, hipRoll, lean, spineTwist, headTilt, headNod;
        public Vector3 upperArm, foreArm, hand; // direções do braço esquerdo (o direito é espelhado)
        public float rightArmDrop;
        public float hipPitch;            // tomba o corpo todo para frente (+) ou para trás (-)
        public Vector3 lFoot, rFoot;      // alvo do tornozelo, relativo ao chão sob o quadril
        public float lPitch, rPitch;      // inclinação dos pés
        public Vector3 knee;              // para onde o joelho dobra (zero = para frente)
    }

    class Rig
    {
        public Transform animRoot, hips, neck, head;
        public Transform[] spine;
        public Transform lUp, lLow, lFoot, rUp, rLow, rFoot;
        public Transform lArm, lFore, lHand, lFinger, rArm, rFore, rHand, rFinger;
        public float thigh, shin, ground;
        public Vector3 hipsRest;
        public Quaternion lFootRest, rFootRest;
        public Dictionary<Transform, (Vector3 p, Quaternion r)> rest = new();
        public List<Transform> animated = new();
    }

    [MenuItem("Tools/Cemetery Gates/Gerar animações do zumbi")]
    public static void Bake()
    {
        var root = PrefabUtility.LoadPrefabContents(PrefabPath);
        try
        {
            root.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            var rig = BuildRig(root);
            var walk = Record(rig, WalkPeriod, true, t => WalkFrame(rig, t));
            var attack = Record(rig, AttackLength, false, t => AttackFrame(rig, t));
            var death = Record(rig, DeathLength, false, t => DeathFrame(rig, t));
            walk = SaveClip(walk, "Assets/Animations/ZombieWalk.anim");
            attack = SaveClip(attack, "Assets/Animations/ZombieAttack.anim");
            death = SaveClip(death, "Assets/Animations/ZombieDeath.anim");
            SetState("Walk", walk);
            SetState("Attack", attack);
            SetState("Death", death);
            AssetDatabase.SaveAssets();
            Debug.Log("Animações do zumbi geradas: ZombieWalk, ZombieAttack e ZombieDeath");
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(root);
        }
    }

    static Rig BuildRig(GameObject root)
    {
        var bones = new Dictionary<string, Transform>();
        foreach (var t in root.GetComponentsInChildren<Transform>())
        {
            int cut = t.name.LastIndexOf('_');
            if (t.name.StartsWith("mixamorig:") && cut > 0) bones[t.name.Substring(10, cut - 10)] = t;
        }
        var rig = new Rig
        {
            animRoot = root.GetComponentInChildren<Animator>().transform,
            hips = bones["Hips"], neck = bones["Neck"], head = bones["Head"],
            spine = new[] { bones["Spine"], bones["Spine1"], bones["Spine2"] },
            lUp = bones["LeftUpLeg"], lLow = bones["LeftLeg"], lFoot = bones["LeftFoot"],
            rUp = bones["RightUpLeg"], rLow = bones["RightLeg"], rFoot = bones["RightFoot"],
            lArm = bones["LeftArm"], lFore = bones["LeftForeArm"], lHand = bones["LeftHand"], lFinger = bones["LeftHandMiddle1"],
            rArm = bones["RightArm"], rFore = bones["RightForeArm"], rHand = bones["RightHand"], rFinger = bones["RightHandMiddle1"],
        };
        foreach (var t in root.GetComponentsInChildren<Transform>()) rig.rest[t] = (t.localPosition, t.localRotation);
        rig.thigh = Vector3.Distance(rig.lUp.position, rig.lLow.position);
        rig.shin = Vector3.Distance(rig.lLow.position, rig.lFoot.position);
        rig.ground = (rig.lFoot.position.y + rig.rFoot.position.y) * 0.5f;
        rig.hipsRest = rig.hips.position;
        rig.lFootRest = rig.lFoot.rotation;
        rig.rFootRest = rig.rFoot.rotation;
        rig.animated.AddRange(new[] { rig.hips, rig.neck, rig.head, rig.lUp, rig.lLow, rig.lFoot, rig.rUp, rig.rLow, rig.rFoot,
            rig.lArm, rig.lFore, rig.lHand, rig.rArm, rig.rFore, rig.rHand });
        rig.animated.AddRange(rig.spine);
        return rig;
    }

    static void WalkFrame(Rig rig, float t)
    {
        float p = t / WalkPeriod * Mathf.PI * 2f;
        float stride = WalkSpeed * WalkPeriod / 4f; // pé de apoio anda para trás na mesma velocidade do corpo
        var pose = new Pose
        {
            hips = new Vector3(-0.03f * Mathf.Sin(p), 0.03f + 0.02f * Mathf.Cos(2f * p), 0f),
            hipTwist = 6f * Mathf.Cos(p),
            hipRoll = 4f * Mathf.Sin(p),
            lean = 16f,
            spineTwist = -5f * Mathf.Cos(p),
            headTilt = 10f + 3f * Mathf.Sin(p),
            headNod = 6f + 3f * Mathf.Sin(2f * p),
            upperArm = new Vector3(-0.12f, -0.25f + 0.07f * Mathf.Sin(p), 1f),
            foreArm = new Vector3(-0.02f, -0.12f, 1f),
            hand = new Vector3(0f, -0.8f, 1f),
            rightArmDrop = 0.12f + 0.07f * Mathf.Sin(p),
        };
        // perna esquerda passo normal; direita arrasta (pouco levantamento, ponta do pé para baixo)
        (pose.lFoot, pose.lPitch) = FootCycle(t / WalkPeriod, stride, 0.11f, 0f);
        (pose.rFoot, pose.rPitch) = FootCycle(t / WalkPeriod + 0.5f, stride, 0.035f, 18f);
        pose.lFoot.x = -0.14f;
        pose.rFoot.x = 0.15f;
        Apply(rig, pose);
    }

    static (Vector3, float) FootCycle(float cycle, float stride, float lift, float dragPitch)
    {
        float u = cycle - Mathf.Floor(cycle);
        if (u < 0.5f) return (new Vector3(0f, 0f, stride - u / 0.5f * 2f * stride), 0f);
        float w = (u - 0.5f) / 0.5f;
        float arc = Mathf.Sin(Mathf.PI * w);
        return (new Vector3(0f, lift * arc, -stride + 2f * stride * Mathf.SmoothStep(0f, 1f, w)), dragPitch * arc);
    }

    static void AttackFrame(Rig rig, float t)
    {
        var ready = new Pose
        {
            hips = new Vector3(0f, 0.03f, 0f), lean = 16f, headTilt = 8f, headNod = 6f,
            upperArm = new Vector3(-0.12f, -0.25f, 1f), foreArm = new Vector3(-0.02f, -0.12f, 1f), hand = new Vector3(0f, -0.8f, 1f),
            rightArmDrop = 0.1f,
        };
        var windup = new Pose
        {
            hips = new Vector3(0f, 0.05f, -0.05f), hipTwist = -8f, lean = -4f, spineTwist = -10f, headTilt = 4f, headNod = -10f,
            upperArm = new Vector3(-0.35f, 0.85f, -0.1f), foreArm = new Vector3(-0.1f, 0.7f, 0.6f), hand = new Vector3(0f, 0.4f, 1f),
        };
        var strike = new Pose
        {
            hips = new Vector3(0f, -0.03f, 0.1f), hipTwist = 6f, lean = 34f, spineTwist = 8f, headTilt = 6f, headNod = 18f,
            upperArm = new Vector3(-0.1f, -0.55f, 1f), foreArm = new Vector3(0.05f, -0.9f, 0.6f), hand = new Vector3(0f, -1f, 0.3f),
        };
        var follow = strike;
        follow.lean = 30f;
        follow.upperArm = new Vector3(-0.12f, -0.8f, 0.7f);
        follow.foreArm = new Vector3(0.05f, -1f, 0.3f);

        var pose = Keyed(t, (0f, ready), (0.38f, windup), (0.5f, strike), (0.75f, follow), (AttackLength, ready));
        pose.lFoot = new Vector3(-0.15f, 0f, 0.2f);
        pose.rFoot = new Vector3(0.16f, 0f, -0.15f);
        Apply(rig, pose);
    }

    // Recua com o impacto, os joelhos cedem e cai de bruços.
    static void DeathFrame(Rig rig, float t)
    {
        var start = new Pose
        {
            hips = new Vector3(0f, 0.03f, 0f), lean = 16f, headTilt = 8f, headNod = 6f,
            upperArm = new Vector3(-0.12f, -0.25f, 1f), foreArm = new Vector3(-0.02f, -0.12f, 1f), hand = new Vector3(0f, -0.8f, 1f),
            rightArmDrop = 0.1f, lFoot = new Vector3(-0.15f, 0f, 0.1f), rFoot = new Vector3(0.16f, 0f, -0.1f),
        };
        var recoil = new Pose
        {
            hips = new Vector3(0f, 0.02f, -0.1f), hipPitch = -8f, hipTwist = 10f, lean = -15f, headTilt = -6f, headNod = -25f,
            upperArm = new Vector3(-0.5f, 0.2f, 0.3f), foreArm = new Vector3(-0.3f, 0.5f, 0.6f), hand = new Vector3(0f, 0.3f, 1f),
            lFoot = new Vector3(-0.15f, 0f, 0.05f), rFoot = new Vector3(0.16f, 0f, -0.15f),
        };
        var kneel = new Pose
        {
            hips = new Vector3(0f, -0.42f, 0.05f), hipPitch = 10f, lean = 20f, headTilt = 12f, headNod = 35f,
            upperArm = new Vector3(-0.15f, -1f, 0.15f), foreArm = new Vector3(-0.05f, -1f, 0.25f), hand = new Vector3(0f, -1f, 0.2f),
            lFoot = new Vector3(-0.15f, 0f, -0.2f), rFoot = new Vector3(0.16f, 0f, -0.3f), lPitch = 30f, rPitch = 30f,
        };
        var prone = new Pose
        {
            hips = new Vector3(0.05f, -0.68f, 0.55f), hipPitch = 82f, hipRoll = -6f, lean = 4f, headTilt = 25f, headNod = 10f,
            upperArm = new Vector3(-0.35f, -0.25f, 1f), foreArm = new Vector3(-0.1f, -0.2f, 1f), hand = new Vector3(0f, -0.5f, 1f),
            rightArmDrop = 0.15f,
            lFoot = new Vector3(-0.2f, 0f, -0.45f), rFoot = new Vector3(0.22f, 0f, -0.38f), lPitch = 75f, rPitch = 70f,
            knee = Vector3.down,
        };
        var settled = prone;
        settled.hips.y -= 0.02f;
        Apply(rig, Keyed(t, (0f, start), (0.25f, recoil), (0.75f, kneel), (1.25f, prone), (DeathLength, settled)));
    }

    static Pose Keyed(float t, params (float time, Pose pose)[] keys)
    {
        int k = 0;
        while (k < keys.Length - 2 && t > keys[k + 1].time) k++;
        float f = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(keys[k].time, keys[k + 1].time, t));
        return Lerp(keys[k].pose, keys[k + 1].pose, f);
    }

    static Pose Lerp(Pose a, Pose b, float f) => new Pose
    {
        hips = Vector3.Lerp(a.hips, b.hips, f),
        hipTwist = Mathf.Lerp(a.hipTwist, b.hipTwist, f),
        hipRoll = Mathf.Lerp(a.hipRoll, b.hipRoll, f),
        lean = Mathf.Lerp(a.lean, b.lean, f),
        spineTwist = Mathf.Lerp(a.spineTwist, b.spineTwist, f),
        headTilt = Mathf.Lerp(a.headTilt, b.headTilt, f),
        headNod = Mathf.Lerp(a.headNod, b.headNod, f),
        upperArm = Vector3.Slerp(a.upperArm.normalized, b.upperArm.normalized, f),
        foreArm = Vector3.Slerp(a.foreArm.normalized, b.foreArm.normalized, f),
        hand = Vector3.Slerp(a.hand.normalized, b.hand.normalized, f),
        rightArmDrop = Mathf.Lerp(a.rightArmDrop, b.rightArmDrop, f),
        hipPitch = Mathf.Lerp(a.hipPitch, b.hipPitch, f),
        lFoot = Vector3.Lerp(a.lFoot, b.lFoot, f),
        rFoot = Vector3.Lerp(a.rFoot, b.rFoot, f),
        lPitch = Mathf.Lerp(a.lPitch, b.lPitch, f),
        rPitch = Mathf.Lerp(a.rPitch, b.rPitch, f),
        knee = Vector3.Lerp(Knee(a), Knee(b), f),
    };

    static Vector3 Knee(Pose p) => p.knee == Vector3.zero ? Vector3.forward : p.knee;

    static void Apply(Rig rig, Pose pose)
    {
        foreach (var kv in rig.rest) kv.Key.SetLocalPositionAndRotation(kv.Value.p, kv.Value.r);

        rig.hips.position = rig.hipsRest + pose.hips;
        rig.hips.rotation = Quaternion.AngleAxis(pose.hipTwist, Vector3.up) * Quaternion.AngleAxis(pose.hipRoll, Vector3.forward)
            * Quaternion.AngleAxis(pose.hipPitch, Vector3.right) * rig.hips.rotation;
        foreach (var s in rig.spine)
            s.rotation = Quaternion.AngleAxis(pose.spineTwist / 3f, Vector3.up) * Quaternion.AngleAxis(pose.lean / 3f, Vector3.right) * s.rotation;
        rig.head.rotation = Quaternion.AngleAxis(pose.headTilt, Vector3.forward) * Quaternion.AngleAxis(pose.headNod, Vector3.right) * rig.head.rotation;

        var ground = new Vector3(rig.hipsRest.x, rig.ground, rig.hipsRest.z);
        LegIK(rig, rig.lUp, rig.lLow, rig.lFoot, ground + pose.lFoot, Knee(pose));
        LegIK(rig, rig.rUp, rig.rLow, rig.rFoot, ground + pose.rFoot, Knee(pose));
        rig.lFoot.rotation = Quaternion.AngleAxis(pose.lPitch, Vector3.right) * rig.lFootRest;
        rig.rFoot.rotation = Quaternion.AngleAxis(pose.rPitch, Vector3.right) * rig.rFootRest;

        Vector3 Mirror(Vector3 v, float drop) => new Vector3(-v.x, v.y - drop, v.z);
        Aim(rig.lArm, rig.lFore, pose.upperArm);
        Aim(rig.lFore, rig.lHand, pose.foreArm);
        Aim(rig.lHand, rig.lFinger, pose.hand);
        Aim(rig.rArm, rig.rFore, Mirror(pose.upperArm.normalized, pose.rightArmDrop));
        Aim(rig.rFore, rig.rHand, Mirror(pose.foreArm.normalized, pose.rightArmDrop * 0.5f));
        Aim(rig.rHand, rig.rFinger, Mirror(pose.hand, 0f));
    }

    static void Aim(Transform bone, Transform child, Vector3 direction) =>
        bone.rotation = Quaternion.FromToRotation(child.position - bone.position, direction) * bone.rotation;

    // IK de 2 ossos: o joelho dobra na direção kneeDir.
    static void LegIK(Rig rig, Transform up, Transform low, Transform foot, Vector3 target, Vector3 kneeDir)
    {
        float a = rig.thigh, b = rig.shin;
        Vector3 hip = up.position;
        Vector3 toTarget = target - hip;
        float d = Mathf.Clamp(toTarget.magnitude, 0.05f, a + b - 0.001f);
        Vector3 dir = toTarget.normalized;
        float angle = Mathf.Acos(Mathf.Clamp((a * a + d * d - b * b) / (2f * a * d), -1f, 1f));
        Vector3 pole = Vector3.ProjectOnPlane(kneeDir, dir).normalized;
        Vector3 knee = hip + (dir * Mathf.Cos(angle) + pole * Mathf.Sin(angle)) * a;
        Aim(up, low, knee - hip);
        Aim(low, foot, hip + dir * d - low.position);
    }

    static AnimationClip Record(Rig rig, float length, bool loop, System.Action<float> pose)
    {
        var curves = new Dictionary<(Transform, string), AnimationCurve>();
        AnimationCurve Curve(Transform t, string prop) =>
            curves.TryGetValue((t, prop), out var c) ? c : curves[(t, prop)] = new AnimationCurve();

        int frames = Mathf.RoundToInt(length * Fps);
        for (int i = 0; i <= frames; i++)
        {
            float t = i / (float)Fps;
            pose(loop && i == frames ? 0f : t); // último quadro = primeiro, para o loop fechar
            foreach (var bone in rig.animated)
            {
                var q = bone.localRotation;
                Curve(bone, "m_LocalRotation.x").AddKey(t, q.x);
                Curve(bone, "m_LocalRotation.y").AddKey(t, q.y);
                Curve(bone, "m_LocalRotation.z").AddKey(t, q.z);
                Curve(bone, "m_LocalRotation.w").AddKey(t, q.w);
            }
            var hp = rig.hips.localPosition;
            Curve(rig.hips, "m_LocalPosition.x").AddKey(t, hp.x);
            Curve(rig.hips, "m_LocalPosition.y").AddKey(t, hp.y);
            Curve(rig.hips, "m_LocalPosition.z").AddKey(t, hp.z);
        }

        var clip = new AnimationClip { frameRate = Fps };
        foreach (var kv in curves)
            clip.SetCurve(AnimationUtility.CalculateTransformPath(kv.Key.Item1, rig.animRoot), typeof(Transform), kv.Key.Item2, kv.Value);
        clip.EnsureQuaternionContinuity();
        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = loop;
        AnimationUtility.SetAnimationClipSettings(clip, settings);
        return clip;
    }

    static AnimationClip SaveClip(AnimationClip clip, string path)
    {
        var existing = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
        if (!existing)
        {
            AssetDatabase.CreateAsset(clip, path);
            return clip;
        }
        EditorUtility.CopySerialized(clip, existing); // mantém o GUID
        existing.name = System.IO.Path.GetFileNameWithoutExtension(path);
        EditorUtility.SetDirty(existing);
        return existing;
    }

    static void SetState(string name, AnimationClip clip)
    {
        var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);
        var machine = controller.layers[0].stateMachine;
        AnimatorState state = null;
        foreach (var child in machine.states) if (child.state.name == name) state = child.state;
        state ??= machine.AddState(name);
        state.motion = clip;
        EditorUtility.SetDirty(controller);
    }
}
