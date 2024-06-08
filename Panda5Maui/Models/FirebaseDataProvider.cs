using Firebase.Auth;
using Firebase.Auth.Providers;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Panda5Maui.Models;

class FirebaseDataProvider : ILoginRequiredService, IDatabaseProvider
{
    private FirebaseAuthClient AuthClient;
    private UserCredential? Credentials;
    private FirestoreDb? Database;

    public FirebaseDataProvider(string apiKey, string authDomain)
    {
        AuthClient = new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = apiKey,
            AuthDomain = authDomain,
            Providers = new FirebaseAuthProvider[] { new EmailProvider() }
        });
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        if (Credentials != null) return "User is already logged in";
        try
        {
            Credentials = await AuthClient.SignInWithEmailAndPasswordAsync(email, password);
            Database = GetFirestoreDbFromUserToken(Credentials);
        }
        catch (Exception e)
        {
            return FormatFirebaseException(e);
        }
        return null;
    }

    public async Task<string?> RegisterAsync(string username, string password, string email)
    {
        if (Credentials != null) return "User is already logged in";
        try
        {
            Credentials = await AuthClient.CreateUserWithEmailAndPasswordAsync(email, password, username);
            Database = GetFirestoreDbFromUserToken(Credentials);
        }
        catch (Exception e)
        {
            return FormatFirebaseException(e);
        }
        return null;
    }

    //Try / catch
    public async Task<Dictionary<string, object>> GetUserdata()
    {
        if (Database == null || Credentials == null) throw new Exception("You must login before accesing the data");
        DocumentSnapshot snapshot = await Database.Collection("UserData").Document(Credentials.User.Uid).GetSnapshotAsync();
        return snapshot.ToDictionary();
    }

    public async Task<bool> SetUserdata(Dictionary<string, object> userdata)
    {
        try
        {
            if (Database == null || Credentials == null) throw new Exception("You must login before accesing the data");
            await Database.Collection("UserData").Document(Credentials.User.Uid).SetAsync(userdata, SetOptions.Overwrite);
        }
        catch { return false; }
        return true;
    }

    private FirestoreDb GetFirestoreDbFromUserToken(UserCredential userCredentials)
    {
        var firebaseCredentials = userCredentials.User.Credential;
        var callCredentials = CallCredentials.FromInterceptor(async (context, metadata) =>
        {
            if (string.IsNullOrEmpty(firebaseCredentials.IdToken)) throw new ArgumentException($"{nameof(userCredentials)} token was empty");
            string token = firebaseCredentials.IsExpired() ? firebaseCredentials.RefreshToken : firebaseCredentials.IdToken;

            metadata.Clear();
            metadata.Add("authorization", $"Bearer {token}");
        });

        var channelCredentials = ChannelCredentials.Create(new SslCredentials(), callCredentials);
        FirestoreClientBuilder builder = new() { ChannelCredentials = channelCredentials };

        return FirestoreDb.Create("panda5-668c8", builder.Build());
    }

    private string FormatFirebaseException(Exception e)
    {
        if (e is FirebaseAuthHttpException firebaseException)
        {
            if (firebaseException.Reason != AuthErrorReason.Unknown) return $"An error has been encountered ({firebaseException.Reason})";

            var response = JsonSerializer.Deserialize<FirebaseAuthHttpExceptionResponse>(firebaseException.ResponseData);
            return response != null ? response.error.message : firebaseException.ResponseData;
        }
        else if (e is FirebaseAuthWithCredentialException firebaseAuthWithCredentialException) return $"Credentials are invalid ({firebaseAuthWithCredentialException.Reason})";
        else if (e is FirebaseAuthException firebaseAuthException) return $"An error has been encountered ({firebaseAuthException.Reason})";
        else return $"An error has been encountered ({e.Message})";
    }
}

