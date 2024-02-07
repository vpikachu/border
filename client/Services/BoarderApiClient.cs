using System.Net.Http.Json;
using boarder.client.Models;
using MudBlazor.Extensions;

namespace boarder.client.Services {
  public class BoarderApiClient {
    HttpClient _httpClient;
    public BoarderApiClient(HttpClient httpClient) {
      _httpClient = httpClient;
    }
    public async virtual Task<Models.Board[]> GetBoards() {
      var response = await _httpClient.GetFromJsonAsync<Models.Board[]>("/boards");
      return response ?? Array.Empty<Board>();
    }
  }
}