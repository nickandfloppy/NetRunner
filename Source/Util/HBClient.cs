using System;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json;

public class HasteBinClient : IDisposable {
	private readonly HttpClient _httpClient;
	private readonly UriBuilder _uriBuilder;
	
	public HasteBinClient(string baseUrl) {
		_httpClient = new HttpClient();
		_uriBuilder = new UriBuilder(baseUrl);
	}
	
	public async Task<HasteBinResult> Post(string content) {
		_uriBuilder.Path = "/documents";
		string postUrl = _uriBuilder.Uri.ToString();

		var requestContent = new StringContent(content);
		var response = await _httpClient.PostAsync(postUrl, requestContent);
		response.EnsureSuccessStatusCode();
		
		string json = await response.Content.ReadAsStringAsync();
		var hasteBinResult = JsonConvert.DeserializeObject<HasteBinResult>(json);
		hasteBinResult.FullUrl = $"{_uriBuilder.Uri}/{hasteBinResult.Key}";
		hasteBinResult.IsSuccess = true;
		hasteBinResult.StatusCode = (int)response.StatusCode;
		return hasteBinResult;
	}
	
	public void Dispose() {
		_httpClient.Dispose();
	}
}

public class HasteBinResult {
	public string Key { get; set; }
	public string FullUrl { get; set; }
	public bool IsSuccess { get; set; }
	public int StatusCode { get; set; }
}
