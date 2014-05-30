$answers = "As I see it, yes",
            "Reply hazy, try again",
            "Outlook not so good."
            
function Get-Answer($question) {
    $answers | Get-Random
}

function Sharp-Update() {
	& 'SharpMigrator.exe'
}

Register-TabExpansion 'Get-Answer' @{
    'question' = {
        "Is Scott's head getting too big?",
        "Is the talk going well?",
        "Will this demo work?"
    }
}

Export-ModuleMember Get-Answer
Export-ModuleMember Sharp-Update