// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Core.Managers;
// using TMPro;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
//
// namespace LoaderLogic
// {
//     public class GameLoader : MonoBehaviour
//     {
//         [SerializeField] private GameLoaderUI loaderUI;
//
//         private void Start()
//         {
//             StartLoadingAsync();
//             loaderUI.Init(100);
//         }
//
//         private void StartLoadingAsync()
//         {
//             DontDestroyOnLoad(gameObject);
//             DontDestroyOnLoad(loaderUI.transform.root.gameObject); 
//             LoadCoreManager();
//         }
//
//         private void LoadCoreManager()
//         {
//             new CoreManager(OnCoreManagersLoaded);
//         }
//
//         private void OnCoreManagersLoaded(bool isSuccess)
//         {
//             if (isSuccess)
//             {
//                 loaderUI.AddProgress(20);
//
//                 StartCoroutine(LoadCharactersLoader());
//             }
//             else
//             {
//                 Debug.Log(new Exception("CoreManager failed to load"));
//             }
//         }
//
//
//         private IEnumerator LoadCharactersLoader()
//         {
//             int count = 0;
//             while (count < 30)  // dummy coroutine for 3 seconds
//             {
//                 loaderUI.AddProgress(1);
//                 yield return new WaitForSeconds(0.1f);
//                 count++;
//             }
//
//             SceneManager.sceneLoaded += OnCharacterLoadSceneLoaded;
//             SceneManager.LoadScene("CharacterLoader");
//         }
//
//         private void OnCharacterLoadSceneLoaded
//             (Scene scene, LoadSceneMode mode)
//         {
//             SceneManager.sceneLoaded -= OnCharacterLoadSceneLoaded;
//             loaderUI.AddProgress(30);
//             OnLoadComplete();
//         }
//
//         private void OnLoadComplete()
//         {
//             Destroy(loaderUI.transform.root.gameObject);
//             Destroy(this.gameObject);
//         }
//     }
// }