using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using VibeWave.DataAccess.Repository.IRepository;
using VibeWave.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Google.GenAI;
using Google.GenAI.Types;
using System.Collections.Generic;
using System;

namespace VibeWave.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IConfiguration _configuration;

        //private static string _apiKey;

        //private readonly HttpClient _httpClient;

        private static List<Content> _chatHistory = new List<Content>();

        // 简单会话记忆（仅保留最近 5 条消息）
        //private static List<ChatMessage> _chatHistory = new List<ChatMessage>
        //{
        //    new ChatMessage { Role = "system", Content = "You are a helpful assistant for VibeWave concert booking. Help users with concerts, tickets, and general information." }
        //};

        public ChatController(IConfiguration configuration)
        {
            _configuration = configuration;
            //_httpClient = new HttpClient();
        }

        //private readonly IUnitOfWork _unitOfWork;
        //public ChatController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}

        //AI

        [HttpGet]
        public IActionResult Index()
        {
            if (TempData["ResponseMessage"] != null)
            {
                ViewBag.Response = TempData["ResponseMessage"].ToString();
            }
            if (TempData["error"] != null)
            {
                ViewBag.Error = TempData["error"].ToString();
            }
            return View(new ChatInput());
        }



        //Made by myself
        [HttpPost]
        public async Task<IActionResult> Index(ChatInput obj)
        {
            if (string.IsNullOrWhiteSpace(obj.UserMessage))
            {
                ModelState.AddModelError("UserMessage", "Message cannot be empty.");

                //if (ModelState.IsValid)
                //{
                //    var botResponse = GenerateResponse(obj.UserMessage);

                //    TempData["success"] = "Message sent to bot.";
                //    TempData["ResponseMessage"] = botResponse;
                //    return RedirectToAction("Index");
                //}
                return View(obj);
            }


            //// 将用户消息加入历史
            //_chatHistory.Add(new ChatMessage { Role = "user", Content = obj.UserMessage });
            //TrimHistory(); // 保留最近 N 条消息，避免 Token 过多


            var apiKey = _configuration["Gemini:ApiKey"];
            //var model = _configuration["OpenAI:Model"] ?? "gpt-3.5-turbo";

            if (string.IsNullOrEmpty(apiKey))
            {
                TempData["error"] = "AI service is not configured. Please contact the administrator.";
                return RedirectToAction("Index");
            }

            _chatHistory.Add(new Content
            {
                Role = "user",
                Parts = new List<Part> { new Part { Text = obj.UserMessage } }
            });

            try
            {
                // 创建 Gemini 客户端
                var client = new Client(apiKey: apiKey);
                var response = await client.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash",
                    contents: _chatHistory,
                    config: new GenerateContentConfig
                    {
                        SystemInstruction = new Content
                        {
                            Parts = new List<Part>
                            {
                                new Part { Text = "You are a helpful assistant for VibeWave concert booking. Help users with concerts, tickets, and general information." }
                            }
                        },
                        Temperature = 0.7f,
                        MaxOutputTokens = 500
                    }
                );

                var replyText = response.Candidates?[0]?.Content?.Parts?[0]?.Text
                                ?? "I couldn't process that. Please try again.";

                // 将模型回复加入历史
                _chatHistory.Add(new Content
                {
                    Role = "model",
                    Parts = new List<Part> { new Part { Text = replyText } }
                });

                TempData["success"] = "Message sent to bot.";
                TempData["ResponseMessage"] = replyText;
            }
            catch (Exception ex)
            {
                TempData["error"] = $"AI service error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    



        //    var requestBody = new ChatGptRequest
        //    {
        //        Model = model,
        //        Messages = _chatHistory
        //    };

        //    var requestJson = JsonSerializer.Serialize(requestBody);
        //    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        //    try
        //    {
        //        _httpClient.DefaultRequestHeaders.Authorization =
        //            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

        //        var response = await _httpClient.PostAsync(
        //            "https://api.openai.com/v1/chat/completions", content);

        //        var responseString = await response.Content.ReadAsStringAsync();

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var chatGptResponse = JsonSerializer.Deserialize<ChatGptResponse>(responseString);
        //            var assistantMessage = chatGptResponse?.Choices?[0]?.Message?.Content
        //                ?? "I'm sorry, I couldn't process that.";

        //            // 将助手回复加入历史
        //            _chatHistory.Add(new ChatMessage { Role = "assistant", Content = assistantMessage });

        //            TempData["success"] = "Message sent to bot.";
        //            TempData["ResponseMessage"] = assistantMessage;
        //        }
        //        else
        //        {
        //            TempData["error"] = "AI service returned an error. Please try again later.";
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        TempData["error"] = "Failed to connect to AI service.";
        //    }

        //    return RedirectToAction("Index");
        //}

        ///// <summary>
        ///// 保持对话历史不超过限制（system + 最近 4 轮对话 = 9 条消息）
        ///// </summary>
        //private void TrimHistory()
        //{
        //    if (_chatHistory.Count > 9)
        //    {
        //        var systemMsg = _chatHistory[0];
        //        var recent = _chatHistory.GetRange(_chatHistory.Count - 8, 8);
        //        _chatHistory.Clear();
        //        _chatHistory.Add(systemMsg);
        //        _chatHistory.AddRange(recent);
        //    }
        //}
    

        

        //private string GenerateResponse(string userMessage)
        //{
        //    string msg = userMessage.ToLower();

        //    if (msg.Contains("hello"))
        //    {
        //        return "Hi there! How can I help you?";
        //    }
        //    else if (msg.Contains("help"))
        //    {
        //        return "You can ask me about our concerts, tickets, or contact information.";
        //    }
        //    else if (msg.Contains("concert"))
        //    {
        //        return "We have various concerts available. Check our homepage for the list!";
        //    }
        //    else if (msg.Contains("ticket"))
        //    {
        //        return "You can book tickets from the concert details page.";
        //    }
        //    else if (msg.Contains("contact"))
        //    {
        //        return "You can reach us via the Contact Us page.";
        //    }
        //    else if (msg.Contains("price"))
        //    {
        //        return "Ticket prices vary by concert. Please see individual concert pages.";
        //    }
        //    else
        //    {
        //        return "I'm not sure how to answer that. Try asking about 'concert', 'ticket', 'help', or 'contact'.";
        //    }
        //}
    }
}
