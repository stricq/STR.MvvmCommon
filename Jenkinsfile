pipeline {
  agent any
  environment {
    MASTER_VER  = '3.0.0'
    RELEASE_VER = '3.0.0'

    GIT_HASH = GIT_COMMIT.take(7)
  }
  stages {
  	stage('Environment') {
  		steps {
        script {
          env.JDATE = new Date().format("yyDDD.HHmm")
        }
  		}
  	}
    stage('Build Debug') {
      when { not { anyOf { branch 'master'; branch 'release' } } }
      steps {
        bat 'dotnet clean --configuration Debug'
        bat 'dotnet build --configuration Debug'
      }
    }
    stage('Build Master') {
      when { branch 'master' }
      steps {
        bat 'dotnet clean --configuration Release'
        bat 'dotnet build --configuration Release -p:Version="%MASTER_VER%-beta.%JDATE%+%GIT_HASH%"'
      }
    }
    stage('Build Release') {
      when { branch 'release' }
      steps {
        bat 'dotnet clean --configuration Release'
        bat 'dotnet build --configuration Release -p:Version="%RELEASE_VER%+%GIT_HASH%"'
      }
    }
    stage('Backup') {
      when { anyOf { branch 'master'; branch 'release' } }
      steps {
        bat '''move /Y nupkgs\\*.nupkg "t:\\Nuget Packages"
        exit 0'''
      }
    }
    stage('Pack Master') {
      when { branch 'master' }
      steps {
        bat 'dotnet pack --no-build --no-restore --configuration Release -p:PackageVersion="%MASTER_VER%-beta.%JDATE%+%GIT_HASH%" -p:Version="%MASTER_VER%-beta.%JDATE%+%GIT_HASH%" --output nupkgs'
      }
    }
    stage('Pack Release') {
      when { branch 'release' }
      steps {
        bat 'dotnet pack --no-build --no-restore --configuration Release -p:PackageVersion="%RELEASE_VER%+%GIT_HASH%" -p:Version="%RELEASE_VER%+%GIT_HASH%" --output nupkgs'
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
