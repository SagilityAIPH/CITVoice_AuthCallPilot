Public Module WorkflowManager

    Public Function GetWorkflow(workflowName As String) As ChecklistNode

        Select Case workflowName

            Case "Authentication"
                Return AuthenticationWorkflow.CreateWorkflow()

            Case "CheckIfOutOfScope"
                Return CheckIfOutOfScopeWorkflow.CreateWorkflow()

            Case "ProviderTriage"
                Return ProviderTriageWorkflow.CreateWorkflow()

            Case "Recommendation"
                Return RecommendationWorkflow.CreateWorkflow()

            Case Else
                Throw New Exception("Unknown workflow: " & workflowName)

        End Select

    End Function

End Module