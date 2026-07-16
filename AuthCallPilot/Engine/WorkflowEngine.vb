Public Module WorkflowEngine

    Public Function InitializeCallPilotTree() As ChecklistNode

        Dim q1 As New ChecklistNode("Q1",
            "Was there a response?")
        q1.Instructions.Add(New WorkflowInstruction("Agent receives the call", InstructionType.Normal))
        q1.Instructions.Add(New WorkflowInstruction("Deliver Welcome Script", InstructionType.Action))

        Dim q2 As New ChecklistNode("Q2",
            "Is this a retro request and claim has been processed?")

        q2.Instructions.Add(New WorkflowInstruction("Ask for contact information", InstructionType.Action))
        q2.Instructions.Add(New WorkflowInstruction("Ask for Date of Service", InstructionType.Action))

        Dim q3 As New ChecklistNode("Q3",
            "Is the member authenticated in Genesys?")
        q3.Instructions.Add(New WorkflowInstruction("Ask for reason of the call", InstructionType.Action))

        Dim endGhost As New ChecklistNode("END1", "End")
        endGhost.Instructions.Add(New WorkflowInstruction("Provide Ghost Call Script", InstructionType.Normal))

        Dim endRetro As New ChecklistNode("END2", "End")
        endRetro.Instructions.Add(New WorkflowInstruction("Refer to Reviewing Claim History", InstructionType.Reference))

        Dim endAuth As New ChecklistNode("END3", "End")
        endAuth.Instructions.Add(New WorkflowInstruction("Verify Member", InstructionType.Action))



        Dim endVerify As New ChecklistNode("END4", "End")

        endVerify.Instructions.Add(New WorkflowInstruction("Ask Member ID", InstructionType.Action))
        endVerify.Instructions.Add(New WorkflowInstruction("Verify DOB", InstructionType.Action))
        endVerify.Instructions.Add(New WorkflowInstruction("Verify Zip", InstructionType.Action))
        endVerify.Instructions.Add(New WorkflowInstruction("Enter into CGA", InstructionType.Action))

        q1.YesNode = q2
        q1.NoNode = endGhost

        q2.YesNode = endRetro
        q2.NoNode = q3

        q3.YesNode = endAuth
        q3.NoNode = endVerify

        Return q1

    End Function

End Module