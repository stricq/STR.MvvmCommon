pipeline {
  agent any
  stages {
    stage('Clean') {
      steps {
        bat 'dotnet clean'
      }
    }
    stage('Restore Packages') {
      steps {
        bat 'dotnet restore --verbosity n'
      }
    }
    stage('Build') {
      steps {
        bat 'dotnet build --configuration Debug'
      }
    }
    stage('Backup') {
      when { branch 'master' }
      steps {
        bat '''move /Y nupkgs\\*.nupkg "t:\\Nuget Packages"
        exit 0'''
      }
    }
    stage('Pack') {
      when { branch 'master' }
      steps {
        bat 'dotnet pack --no-build --output nupkgs'
      }
    }
    stage('Publish') {
      when { branch 'master' }
      environment {
        NUGET_API_KEY = credentials('nuget-api-key')
      }
      steps {
        bat 'dotnet nuget push **\\nupkgs\\*.nupkg -k %NUGET_API_KEY% -s https://api.nuget.org/v3/index.json --no-symbols true'
      }
    }
  }
}
