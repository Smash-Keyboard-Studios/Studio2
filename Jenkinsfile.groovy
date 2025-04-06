def PROJECT_NAME = "Studio2"
def CUSTOM_WORKSPACE = "C:\\Jenkins\\Unity_Projects\\${PROJECT_NAME}"
def UNITY_VERSION = "2022.3.44f1"
def UNITY_INSTALLATION = "Unity\\Hub\\Editor\\${UNITY_VERSION}\\Editor"

pipeline{
    environment{
        PROJECT_PATH = "${CUSTOM_WORKSPACE}\\${PROJECT_NAME}"
    }
    agent{
        label{
            label ""
            customWorkspace "${CUSTOM_WORKSPACE}"
        }
    }
    stages{
        stage('Build Windows'){
            when{expression {BUILD_WINDOWS == 'true'}}
            steps{
                script{
                    withEnv(["UNITY_PATH=${UNITY_INSTALLATION}"]){
                        bat '''
                        "%ProgramFiles%/%UNITY_PATH%/Unity.exe" -quit -batchmode -projectPath "%CUSTOM_WORKSPACE%" -executeMethod BuildScript.BuildWindows -logFile -
                        '''
                    
                    }
                }
            }
        }
    
        stage('Deploy Windows'){
            when{expression {DEPLOY_WINDOWS == 'true'}}
            steps{
                echo 'Deploy Windows'
            }
        }
    }

    post{
        always{
            archiveArtifacts artifacts: "/Builds/**", allowEmptyArchive: true
        }
        failure{
            echo 'Build Failed'
        }
    }
}