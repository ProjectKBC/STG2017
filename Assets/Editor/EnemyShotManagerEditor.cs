using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyShotManager), true)]
public class EnemyShotManagerEditor : Editor
{


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EnemyShotManager enemyShotManager = target as EnemyShotManager;
        EnemyBulletParam param = enemyShotManager.param;

        EditorGUILayout.LabelField("共通パラメータ");
        EditorGUI.indentLevel++;

        enemyShotManager.bullet = EditorGUILayout.ObjectField("Bulletクラス", enemyShotManager.bullet, typeof(EnemyBullet), true) as EnemyBullet;
        var speed = serializedObject.FindProperty("param.speed");
        speed.floatValue = EditorGUILayout.FloatField("弾丸の速度", speed.floatValue);
        param.shotDelay = EditorGUILayout.FloatField("ショット間隔", param.shotDelay);
        param.power = EditorGUILayout.FloatField("攻撃力", param.power);
        param.lifeTime = EditorGUILayout.FloatField("生存時間", param.lifeTime);
        param.isPenetrate = EditorGUILayout.Toggle("貫通性", param.isPenetrate);
        param.initialPosition = EditorGUILayout.Vector3Field("ローカル初期位置", param.initialPosition);
        param.shotSound = EditorGUILayout.ObjectField("ショット音", param.shotSound, typeof(AudioClip), true) as AudioClip;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("指定弾数撃った後のショット間隔");
        param.shotDelay2 = EditorGUILayout.FloatField("", param.shotDelay2);
        EditorGUILayout.LabelField("追加遅延のための指定弾数");
        param.delayShotCount = EditorGUILayout.IntField("", param.delayShotCount);

        EditorGUI.indentLevel--;
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("分岐パラメータ");
        EditorGUI.indentLevel++;
        param.shotMovePattern = (ShotMovePattern)EditorGUILayout.EnumPopup("ショットの種類", param.shotMovePattern);

        switch (param.shotMovePattern)
        {
            case ShotMovePattern.Straight:
            case ShotMovePattern.PlayerAim:
                break;

            case ShotMovePattern.EveryDirection:
                param.angleInterval = EditorGUILayout.FloatField("角度間隔", param.angleInterval);
                break;

            case ShotMovePattern.Tornado:
                param.spinSpeed = EditorGUILayout.FloatField("回転速度", param.spinSpeed);
                break;
        }

        EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
    }

}
