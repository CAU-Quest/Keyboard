using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Analyzer))]
public class AnalyzerEditor : Editor
{
    private void OnEnable()
    {
        // Editor 초기화
    }

    public override void OnInspectorGUI()
    {
        // 기본 인스펙터 그리기
        DrawDefaultInspector();

        // Analyzer 인스턴스 가져오기
        Analyzer analyzer = (Analyzer)target;

        // 오류율 리스트가 존재할 때 그래프 그리기
        if (analyzer.errorRates != null && analyzer.errorRates.Count > 0)
        {
            GUILayout.Label("Error Rate Over Time", EditorStyles.boldLabel);

            // 그래프 그리기 영역 설정
            Rect graphRect = GUILayoutUtility.GetRect(400, 200);
            if (Event.current.type == EventType.Repaint)
            {
                // 그래프 배경 그리기
                EditorGUI.DrawRect(graphRect, Color.black);

                // 그래프 내용 그리기
                Handles.BeginGUI();
                Handles.color = Color.green;
                
                float maxErrorRate = Mathf.Max(analyzer.errorRates.ToArray());
                for (int i = 0; i < analyzer.errorRates.Count - 1; i++)
                {
                    Vector3 p1 = new Vector3(graphRect.x + (i * graphRect.width / analyzer.errorRates.Count), 
                                             graphRect.y + (1 - analyzer.errorRates[i] / maxErrorRate) * graphRect.height);
                    Vector3 p2 = new Vector3(graphRect.x + ((i + 1) * graphRect.width / analyzer.errorRates.Count), 
                                             graphRect.y + (1 - analyzer.errorRates[i + 1] / maxErrorRate) * graphRect.height);
                    Handles.DrawLine(p1, p2);
                }

                Handles.EndGUI();

                // x축과 y축 레이블 추가
                GUIStyle labelStyle = new GUIStyle(EditorStyles.label) 
                {
                    alignment = TextAnchor.MiddleCenter, 
                    normal = { textColor = Color.white }
                };

                // y축 레이블 추가
                EditorGUI.LabelField(new Rect(graphRect.x - 50, graphRect.y + graphRect.height / 2 - 10, 40, 20), "Error Rate", labelStyle);
                // x축 레이블 추가
                EditorGUI.LabelField(new Rect(graphRect.x + graphRect.width / 2 - 50, graphRect.y + graphRect.height + 10, 100, 20), "Time (Key Presses)", labelStyle);
            }
        }
    }
}

public class AnalyzerEditorWindow : EditorWindow
{
    private Analyzer analyzer;
    private Vector2 scrollPosition;

    [MenuItem("Window/Analyzer Graph")]
    public static void ShowWindow()
    {
        GetWindow<AnalyzerEditorWindow>("Analyzer Graph");
    }


    private void OnEnable()
    {
        // 씬에서 Analyzer 스크립트 인스턴스를 자동으로 찾기
        analyzer = FindObjectOfType<Analyzer>();
    }
    
    private void OnGUI()
    {
        // Scroll view to handle long lists of error rates
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Select the Analyzer script to monitor
        analyzer = (Analyzer)EditorGUILayout.ObjectField("Analyzer", analyzer, typeof(Analyzer), true);

        if (analyzer == null)
        {
            analyzer = FindObjectOfType<Analyzer>();
        }
        else
        {
            
            if (GUILayout.Button("Copy Error Rates to Clipboard"))
            {
                CopyErrorRatesToClipboard();
            }
            
            // 중앙에 정렬된 제목
            GUILayout.Label("Error Rate Over Time", new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter
            });

            GUILayout.BeginHorizontal();

            // Define space for y-axis label
            Rect yAxisLabelRect = GUILayoutUtility.GetRect(50, 200); // Adjust the width as needed
            EditorGUI.LabelField(new Rect(yAxisLabelRect.x, yAxisLabelRect.y + yAxisLabelRect.height / 2 - 10, yAxisLabelRect.width, 20), "Error Rate", new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            });

            // 그래프 그리기 영역 설정
            Rect graphRect = GUILayoutUtility.GetRect(400, 200);

            if (Event.current.type == EventType.Repaint)
            {
                // 그래프 배경 그리기
                EditorGUI.DrawRect(graphRect, Color.black);

                // 그래프 내용 그리기
                Handles.BeginGUI();
                Handles.color = Color.green;

                float maxErrorRate = Mathf.Max(analyzer.errorRates.ToArray());
                for (int i = 0; i < analyzer.errorRates.Count - 1; i++)
                {
                    Vector3 p1 = new Vector3(graphRect.x + (i * graphRect.width / analyzer.errorRates.Count),
                                             graphRect.y + (1 - analyzer.errorRates[i] / maxErrorRate) * graphRect.height);
                    Vector3 p2 = new Vector3(graphRect.x + ((i + 1) * graphRect.width / analyzer.errorRates.Count),
                                             graphRect.y + (1 - analyzer.errorRates[i + 1] / maxErrorRate) * graphRect.height);
                    Handles.DrawLine(p1, p2);
                }

                Handles.EndGUI();

                // x축 레이블 추가
                GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = Color.white }
                };
                EditorGUI.LabelField(new Rect(graphRect.x + graphRect.width / 2 - 50, graphRect.y + graphRect.height + 10, 100, 20), "Time", labelStyle);
            }

            GUILayout.EndHorizontal();
            
            
        }

        EditorGUILayout.EndScrollView();
    }

    private void CopyErrorRatesToClipboard()
    {
        if (analyzer != null && analyzer.errorRates != null)
        {
            string errorRatesText = string.Join("\t", analyzer.errorRates);
            EditorGUIUtility.systemCopyBuffer = errorRatesText;
            Debug.Log("Error rates copied to clipboard.");
        }
        else
        {
            Debug.LogWarning("No error rates available to copy.");
        }
    }
}