using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShotManager), true)]
public class ShotManagerEditor : Editor
{


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        ShotManager shotManager = target as ShotManager;
        BulletParam param = shotManager.param;
        
        switch (param.shotMode)
        {
            case ShotMode.SimpleShot:
                //EditorGUILayout.HelpBox("", MessageType.Info, true);
                break;

            case ShotMode.ChargeShot:
                //EditorGUILayout.HelpBox("", MessageType.Info, true);
                break;

            case ShotMode.LimitShot:
                //EditorGUILayout.HelpBox("", MessageType.Info, true);
                break;
        }
        
        EditorGUILayout.LabelField("共通パラメータ");
        EditorGUI.indentLevel++;
        param.name = EditorGUILayout.TextField("ショット名", param.name);

        EditorGUILayout.Space();

        shotManager.keyCode = (KeyCode)EditorGUILayout.EnumPopup("入力キー", shotManager.keyCode);
        shotManager.bullet  = EditorGUILayout.ObjectField("Bulletクラス", shotManager.bullet, typeof(Bullet), true) as Bullet;
        param.shotSound     = EditorGUILayout.ObjectField("ショット音", param.shotSound, typeof(AudioClip), true) as AudioClip;

        EditorGUILayout.Space();

        param.shotDelay   = EditorGUILayout.FloatField("ショット間隔",   param.shotDelay);
        param.lifeTime    = EditorGUILayout.FloatField("弾丸の生存時間", param.lifeTime);
        param.speed       = EditorGUILayout.FloatField("弾丸の速度",     param.speed);
        param.power       = EditorGUILayout.FloatField("攻撃力",         param.power);
        param.isPenetrate = EditorGUILayout.Toggle("貫通性", param.isPenetrate);
        param.initialPosition = EditorGUILayout.Vector3Field("ローカル初期位置", param.initialPosition);

        EditorGUI.indentLevel--;
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("分岐パラメータ");
        EditorGUI.indentLevel++;
        param.shotMode = (ShotMode)EditorGUILayout.EnumPopup("ショットの種類", param.shotMode);

        switch (param.shotMode)
        {
            case ShotMode.SimpleShot:
                break;

            case ShotMode.ChargeShot:
                param.chargeTime = EditorGUILayout.FloatField("チャージ時間", param.chargeTime);

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ゲージ");
                EditorGUI.indentLevel++;

                param.gage.barType = (GageBarType)EditorGUILayout.EnumPopup("ゲージバー", param.gage.barType);
                param.gage.effectType = (GageEffectType)EditorGUILayout.EnumPopup("ゲージ描画方法", param.gage.effectType);
                param.gage.countType = (GageCountType)EditorGUILayout.EnumPopup("カウント方法", param.gage.countType);
                break;

            case ShotMode.LimitShot:
                param.bulletMaxNum = EditorGUILayout.IntField("最大弾丸数", param.bulletMaxNum);
                param.reloadTime = EditorGUILayout.FloatField("リロード時間", param.reloadTime);

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("ゲージ");
                EditorGUI.indentLevel++;

                param.gage.barType = (GageBarType)EditorGUILayout.EnumPopup("ゲージバー", param.gage.barType);
                param.gage.effectType = (GageEffectType)EditorGUILayout.EnumPopup("ゲージ描画方法", param.gage.effectType);
                param.gage.countType = (GageCountType)EditorGUILayout.EnumPopup("カウント方法", param.gage.countType);
                break;
        }

        EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
    }

}
