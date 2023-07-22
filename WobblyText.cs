using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WobblyText : MonoBehaviour
{
    [SerializeField] TMP_Text _textComponent;
    [SerializeField] float _wobbleDistance = 10.0f;
    [SerializeField] float _wobbleSpeed = 5.0f;

    private void Update()
    {
        _textComponent.ForceMeshUpdate();   
        TMP_TextInfo textInfo = _textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible) continue; //skip invisible characters

            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                Vector3 original = vertices[charInfo.vertexIndex + j];
                vertices[charInfo.vertexIndex + j] = original + new Vector3(0, Mathf.Sin(Time.time * _wobbleSpeed + i) * _wobbleDistance, 0);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++) //update working mesh
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            _textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
