pipeline {
  agent any
  stages {
    stage('Clean') {
      steps {
        bat 'dotnet clean'
      }
    }
    stage('Build Debug') {
      when { not { branch 'release' } }
      steps {
        bat 'dotnet build --configuration Debug'
      }
    }
    stage('Build Release') {
      when { branch 'release' }
      steps {
        bat 'dotnet build --configuration Release'
      }
    }
    stage('Backup') {
      when { anyOf { branch 'master'; branch 'release' } }
      steps {
        bat '''move /Y nupkgs\\*.nupkg "t:\\Nuget Packages"
        exit 0'''
      }
    }
    stage('Pack Debug') {
      when { branch 'master' }
      steps {
        bat 'dotnet pack --no-build --no-restore --configuration Debug --output nupkgs'
      }
    }
    stage('Pack Release') {
      when { branch 'release' }
      steps {
        bat 'dotnet pack --no-build --no-restore --configuration Release --output nupkgs'
      }
    }
    stage('Publish') {
      when { anyOf { branch 'master'; branch 'release' } }
      environment {
        NUGET_API_KEY = credentials('nuget-api-key')
      }
      steps {
        bat 'dotnet nuget push **\\nupkgs\\*.nupkg -k %NUGET_API_KEY% -s https://api.nuget.org/v3/index.json --no-symbols true'
      }
    }
  }
}
