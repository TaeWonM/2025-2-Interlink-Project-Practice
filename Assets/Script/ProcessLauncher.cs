using System.Diagnostics; // 프로세스 관리를 위해 필요
using System.IO;
using System.Security.Cryptography;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class ProcessLauncher : MonoBehaviour
{
    private Process pythonProcess;

    // 1. Python 실행 파일 경로 (환경 변수에 등록되어 있다면 'python')
    public string pythonExecutable = "python";

    // 2. 실행할 Python 스크립트 파일명 (Unity 프로젝트 내부에 위치)
    public string scriptFileName = "mediapipe_udp_server.py";

    // Python 파일이 Unity Assets 폴더 아래의 'PythonServer' 폴더에 있다고 가정
    private string scriptDirectory = "PythonServer";

    void Start()
    {
        StartPythonServer();
    }

    void StartPythonServer()
    {
        if (pythonProcess != null && !pythonProcess.HasExited) return;

        // Unity의 DataPath 내에 있는 스크립트의 전체 경로를 만듭니다.
        // 예: [UnityProject]/Assets/PythonServer/mediapipe_udp_server.py
        string fullScriptPath = System.IO.Path.Combine(Application.dataPath, scriptDirectory, scriptFileName).Replace('\\', '/');
        if (!File.Exists(fullScriptPath))
        {
            UnityEngine.Debug.LogError("Python 스크립트 파일을 찾을 수 없습니다: " + fullScriptPath);
            return;
        }
        fullScriptPath = string.Format("\"{0}\"", System.IO.Path.Combine(Application.dataPath, scriptDirectory, scriptFileName));

        pythonProcess = new Process();
        pythonProcess.StartInfo.FileName = pythonExecutable;
        pythonProcess.StartInfo.RedirectStandardError = true;
        pythonProcess.StartInfo.RedirectStandardInput = true;
        pythonProcess.StartInfo.RedirectStandardOutput = true;

        // 인수는 'python [스크립트 경로]' 형식이 됩니다.
        pythonProcess.StartInfo.Arguments = fullScriptPath;

        // 콘솔 창을 숨기고 백그라운드에서 실행하도록 설정 (선택 사항)
        pythonProcess.StartInfo.UseShellExecute = false;
        pythonProcess.StartInfo.CreateNoWindow = true;

        try
        {
            bool flag = pythonProcess.Start();
            UnityEngine.Debug.Log(flag + "Python 서버 프로세스 시작 성공.");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Python 서버 시작 실패. Python 경로/환경을 확인하세요: " + e.Message);
            pythonProcess = null;
        }
    }

    // 중요: 유니티가 종료될 때 Python 서버도 함께 종료해야 합니다.
    void OnApplicationQuit()
    {
        StopPythonServer();
    }

    void StopPythonServer()
    {
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            try
            {
                // 프로세스 강제 종료
                pythonProcess.Kill();
                pythonProcess.Dispose();
                pythonProcess = null;
                UnityEngine.Debug.Log("Python 서버 프로세스 종료 완료.");
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError("Python 프로세스 종료 실패: " + e.Message);
            }
        }
    }
}