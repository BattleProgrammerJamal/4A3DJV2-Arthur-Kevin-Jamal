﻿/**
 * @Author : Arthur TORRENTE
 * @Date : 07/12/2014
 * @Desc : Action de chargement d'un niveau
 */

using UnityEngine;
using System.Collections;

public class LoadLevelActionScript : CustomActionScript
{
    public string m_levelName;

    protected override IEnumerator DoActionOnAvent(MonoBehaviour eventSender, GameObject args)
    {
        Application.LoadLevel(m_levelName);

        yield return null;
    }
}
