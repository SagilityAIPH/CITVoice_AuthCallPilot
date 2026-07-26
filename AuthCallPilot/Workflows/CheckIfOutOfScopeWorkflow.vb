Public Module CheckIfOutOfScopeWorkflow

    Public Function CreateWorkflow() As ChecklistNode

        '==========================
        ' Question 1
        '==========================

        Dim q1 As New ChecklistNode(
            "OOS_Q1",
            "Is the member Out of scope?")

        q1.Instructions.Add(New WorkflowInstruction(
            "Refer to Triaging Manila Out of Scope (OOS) Groups",
            InstructionType.Action))

        q1.Instructions.Add(New WorkflowInstruction(
            "Refer to Global Restrictions Out of Scope (OOS) Group List.",
            InstructionType.Action))

        '==========================
        ' YES END
        '==========================

        Dim endYes As New ChecklistNode(
            "OOS_END1",
            "End")

        endYes.Instructions.Add(New WorkflowInstruction(
            "Agent checks the Clinical Directory for the correct phone number and transfers the caller to the appropriate team.",
            InstructionType.Action))

        endYes.Instructions.Add(New WorkflowInstruction(
            "Agent will create CDR and finish the documentation.",
            InstructionType.Action))

        endYes.Instructions.Add(New WorkflowInstruction(
            "End of Process",
            InstructionType.Normal))

        '==========================
        ' Question 2
        '==========================

        Dim q2 As New ChecklistNode(
            "OOS_Q2",
            "Does the member have an active policy for the requested dates?")

        q2.Instructions.Add(New WorkflowInstruction(
            "Agent asks for the Requesting/Treating/Facility NPI and verifies the name, physical and mailing address.",
            InstructionType.Action))

        '==========================
        ' Link
        '==========================

        q1.YesNode = endYes
        q1.NoNode = q2

        Return q1

    End Function

End Module