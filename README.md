# Google 로그인 인증 사용하기

## Google 로그인 구현

https://github.com/googlesamples/google-signin-unity

- Firebase Auth을 사용하기 위해선 사용하려는 Provider (Google, Facebook 등)의 로그인이 선행되어야 합니다.
- Google로 로그인 하는 기능을 사용하기 위해서 Google API를 이용해서 로그인한 이후에 idToken값을 얻어올 필요가 있습니다.

### `The supplied auth credential is malformed or has expired. **Unable to parse Google id_token**`

- google login을 구현하지 않은 채로 Firebase 인증을 구현했을 때 발생한 에러

```bash
2023-12-15 21:29:13.469 4723 5272 Warn System Ignoring header X-Firebase-Locale because its value was null.
2023-12-15 21:29:13.502 4723 5272 Debug TrafficStats tagSocket(6) with statsTag=0xffffffff, statsUid=-1
2023-12-15 21:29:14.124 4723 4845 Info Unity One or more errors occurred. (The supplied auth credential is malformed or has expired. [ Unable to parse Google id_token: /*잘못 입력된 토큰 값*/ ])
```

### `Got Error: DeveloperError Exception of type 'Google.GoogleSignIn+SignInException' was thrown.`

- SHA1 중복 에러 해결 및 SHA256 추가 (실패)
    - 파이어베이스 콘솔 > Android 앱 SDK 설정 및 구성 > SHA1 중복 에러 발생
        - 기존 키스토어 삭제후 재생성한 내용으로 SHA1 교체
        - SHA256 추가로 입력
        
        [Unity Firebase Google SignIn Error Help](https://discussions.unity.com/t/unity-firebase-google-signin-error-help/252175)
        

## Firebase 인증 구현

[Google 로그인과 Unity를 사용하여 인증하기      |  Firebase](https://firebase.google.com/docs/auth/unity/google-signin?hl=ko)

- googleIdToken: 구글 로그인을 통해 받아온 값으로 입력
- googleAccessToken: null

```csharp
Firebase.Auth.Credential credential =
    Firebase.Auth.GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task => {
  if (task.IsCanceled) {
    Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
    return;
  }
  if (task.IsFaulted) {
    Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
    return;
  }

  Firebase.Auth.AuthResult result = task.Result;
  Debug.LogFormat("User signed in successfully: {0} ({1})",
      result.User.DisplayName, result.User.UserId);
});
```

# SHA-1, SHA256 지문 알아내기

1. jre 설치 여부 확인
    1. 미설치 되었다면 설치 진행 ([다운로드)](https://www.java.com/ko/download/)
2. 터미널 실행 (cmd, powershell)
3. jre bin 폴더로 경로 이동

```bash
cd C:\Program Files\Java\jre-1.8\bin
```

1. 지문을 가져오기 위한 커맨드 입력

```bash
keytool -keystore C:\GitHub\unity_title\title.keystore -list -v
```

1. 키 저장소 비밀번호 입력 (keystore 비밀번호 입력)

1. SHA1, SHA256 지문 확인

![Untitled](https://github.com/gimjeonghyeon/unity_playground_title/assets/17286534/702abfe6-6bae-4cec-99a5-169d0d80794e)
