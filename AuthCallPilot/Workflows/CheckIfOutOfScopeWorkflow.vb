Public Module CheckIfOutOfScopeWorkflow

    Public Function CreateWorkflow() As ChecklistNode

        Dim oos_q001 As New ChecklistNode(
            "OOS_Q001",
            "Is the member Out of scope?")

        Dim oos_q002 As New ChecklistNode(
            "OOS_Q002",
            "Does the member have an active policy for the requested dates?")
        oos_q002.Instructions.Add(New WorkflowInstruction(
            "Agent asks for the Requesting/Treating/Facility NPI and verifies the name, physical and mailing address.",
            InstructionType.Action))

        Dim oos_q003 As New ChecklistNode(
            "OOS_Q003",
            "Did the agent find the member in SRO with an active policy?")
        oos_q003.Instructions.Add(New WorkflowInstruction(
            "Agent validates the member’s ID in SRO to confirm if the policy is active.",
            InstructionType.Action))

        Dim oos_end001 As New ChecklistNode(
            "OOS_END001",
            "End")
        oos_end001.Instructions.Add(New WorkflowInstruction(
            "Agent checks the Clinical Directory for the correct phone number and transfers the caller to the appropriate team.",
            InstructionType.Action))
        oos_end001.Instructions.Add(New WorkflowInstruction(
            "Agent will create CDR and finish the documentation.",
            InstructionType.Action))
        oos_end001.Instructions.Add(New WorkflowInstruction(
            "End of Process",
            InstructionType.EndProcess))

        Dim oos_end002 As New ChecklistNode(
            "OOS_END002",
            "Proceed to Provider Triage")
        oos_end002.Instructions.Add(New WorkflowInstruction(
            "Agent proceeds to Provider Triage.",
            InstructionType.Action))

        Dim oos_end003 As New ChecklistNode(
            "OOS_END003",
            "End")
        oos_end003.Instructions.Add(New WorkflowInstruction(
            "Agent will confirm member's policy.",
            InstructionType.Action))
        oos_end003.Instructions.Add(New WorkflowInstruction(
            "End of Process",
            InstructionType.EndProcess))

        Dim oos_end004 As New ChecklistNode(
            "OOS_END004",
            "End")
        oos_end004.Instructions.Add(New WorkflowInstruction(
            "Offer to transfer the caller to Customer Care for benefits verification through Genesys, or coordinate directly with the member.",
            InstructionType.Action))
        oos_end004.Instructions.Add(New WorkflowInstruction(
            "End of Process",
            InstructionType.EndProcess))

        oos_q001.Responses.Add(New WorkflowResponse("YES", oos_end001))
        oos_q001.Responses.Add(New WorkflowResponse("NO", oos_q002))

        oos_q002.Responses.Add(New WorkflowResponse("YES", oos_end002))
        oos_q002.Responses.Add(New WorkflowResponse("NO", oos_q003))

        oos_q003.Responses.Add(New WorkflowResponse("YES", oos_end003))
        oos_q003.Responses.Add(New WorkflowResponse("NO", oos_end004))

        Return oos_q001

    End Function

End Module