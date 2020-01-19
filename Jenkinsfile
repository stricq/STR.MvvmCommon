pipeline {
  agent any
  environment {
    DEBUG_VER   = '3.0.0'
    RELEASE_VER = '3.0.0'

    GIT_HASH = GIT_COMMIT.take(7)
  }
  stages {
  	stage('Environment') {
  		steps {
        script {
          env.JDATE = new Date().format("yyDDDHHmm")
        }
  		}
  	}
    stage('Build Debug') {
      when { not { branch 'release' } }
      steps {
        bat 'dotnet clean --configuration Debug'
        bat 'dotnet build --configuration Debug -p:Version="%DEBUG_VER%-%GIT_BRANCH%.%JDATE%+%GIT_HASH%" -p:PublishRepositoryUrl=true'
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
      steps {
        bat '''move /Y nupkgs\\*.nupkg "t:\\Nuget Packages"
        exit 0'''
      }
    }
    stage('Pack Debug') {
      when { not { branch 'release' } }
      steps {
        bat 'dotnet pack --configuration Debug --no-build --include-symbols -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg -p:PackageVersion="%DEBUG_VER%-%GIT_BRANCH%.%JDATE%+%GIT_HASH%" --output nupkgs'
      }
    }
    stage('Pack Release') {
      when { branch 'release' }
      steps {
        bat 'dotnet pack --configuration Release --no-build -p:PackageVersion="%RELEASE_VER%+%GIT_HASH%" --output nupkgs'
      }
    }
    stage('Publish') {
      environment {
        NUGET_API_KEY = credentials('nuget-api-key')
      }
      steps {
        bat 'dotnet nuget push **\\nupkgs\\*.nupkg -k %NUGET_API_KEY% -s https://api.nuget.org/v3/index.json'
      }
    }
  }
}
