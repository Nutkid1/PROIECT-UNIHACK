using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Teleportingtoroom: MonoBehaviour
{ [SerializeField]
    private float delayBeforeLoading = 10f;
  [SerializeField]
      private string sceneNameToLoad;
        
  private float timeElapsed;
    
   private void Update()
    {timeElapsed += Time.deltaTime ;
   
    if (timeElapsed > delayBeforeLoading ){
        SceneManager.LoadScene (sceneNameToLoad);
    }
        
    }
    
}
