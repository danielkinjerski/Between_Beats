using UnityEngine;
using System.Collections;

public class Facebook : MonoBehaviour {
	
	public const string authorizations_url = "http://caldwell3rdfloor.dyndns.org:3000/authorizations";
	public const string client_id = "150345411773349";
	public const string scope = "publish_stream";
	public const string iconUrl = "http://caldwell3rdfloor.dyndns.org:3000/images/icon.png";
	public const int maxRetries = 10;
	public string state;
	public string token;
	
	public void Publish(string message){
		StartCoroutine(PublishC(message));
	}
	
	protected IEnumerator PublishC(string message){
		if(this.token != ""){
			string url = "https://graph.facebook.com/me/feed";
			
			WWWForm form = new WWWForm();
			form.AddField("access_token", this.token);
			form.AddField("message", message);
			form.AddField("icon", iconUrl);
			form.AddField("link", "http://caldwell3rdfloor.dyndns.org:3000/");
			form.AddField("description", "There are many secrets in the world, but you may have to look at things differently. Play now to find out!");
			
			WWW request = new WWW(url, form);
			
			yield return request;
		}
	}
	
	public string AuthDialogUrl(){
		this.state = System.Guid.NewGuid().ToString();
		string url = "https://www.facebook.com/dialog/oauth?" +
			"client_id=" + client_id +
			"&redirect_uri=" + authorizations_url +
			"&scope=" + scope +
			"&state=" + state;
		
		return url;
	}
	
	public IEnumerator GetTokenFrom(string url){
		int i = 0;
		while(i < maxRetries){
			WWW response = new WWW(url);
			
			yield return response;
			
			JSONObject data = new JSONObject(response.text);
			string error = data["error"].str;
			if(error == "" || error == null){
				this.token = data["token"].str;
                this.gameObject.SendMessageUpwards("SuccessFacebookLink");
            }
            else if (i >= maxRetries)
            {
                this.gameObject.SendMessageUpwards("FailedFacebookLink");
            }
			i++;
		}
	}
	
	void GetToken()
    {
#if UNITY_WEBPLAYER
        Application.ExternalEval("window.open('" + AuthDialogUrl() + "','Connect to Jump Shift')");
        time = Time.timeSinceLevelLoad;
        tokenAccess = TokenAccess.BeginAttempt;
#else
        Application.OpenURL(AuthDialogUrl());     
#endif
        this.gameObject.SendMessageUpwards("ProcessFacebookLink");
        string url = authorizations_url + "/" + this.state;
		
		StartCoroutine(GetTokenFrom(url));
	}

#if UNITY_WEBPLAYER
    public enum TokenAccess
    {
        Null, BeginAttempt, Success, Failed
    }
    TokenAccess tokenAccess = TokenAccess.Null;
    float time;
    void Update()
    {
        if (tokenAccess == TokenAccess.BeginAttempt)
        {
            if (token.Length > 0 && state.Length > 0)
            {
                this.gameObject.SendMessageUpwards("SuccessFacebookLink");
                tokenAccess = TokenAccess.Success;
            }
            else if ((Time.timeSinceLevelLoad - time) % 60 > 5)
            {
                this.gameObject.SendMessageUpwards("FailedFacebookLink");
                tokenAccess = TokenAccess.Failed;
            }
        }
    }
#endif
}
