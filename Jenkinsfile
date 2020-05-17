pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        mstest(failOnError: true, testResultsFile: 'TestResultsFile')
      }
    }

  }
}