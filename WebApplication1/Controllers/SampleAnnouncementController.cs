using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleAnnouncementController : ControllerBase
    {
        public SampleAnnouncementController()
        {
        }

        /// <summary>
        /// 取得所有公告列表
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<AnnouncementsUserView> GetAnnouncementsAsync([FromQuery] GetSomethingParam.Query queryParam)
        {
            Task.Delay(100).Wait();

            var anns = new List<Announcement>()
            {
                new(){ Id=1, Title="第一則公告"},

                new(){ Id=2, Title="第二則公告"}
            };

            var result = new AnnouncementsUserView(anns);

            return result;
        }

        /// <summary>
        /// 根據 ID 取得單一公告
        /// </summary>
        [HttpGet("{id}")]
        public async Task<AnnouncementUserView> GetAnnouncementByIdAsync(int id)
        {
            Task.Delay(100).Wait();

            if (id == 1)
            {
                return new AnnouncementUserView(1, title: "第一則公告", "AAAAAAAAAAAAA", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(3));
            }
            //必須在物件初始設定式或屬性建構函式中設定必要的成員 'AnnouncementDto.Title'。
            if (id == 2)
            {
                return new AnnouncementUserView(2, title: "第二則公告", "BBBBBBBBBBBBBBBBBBBBBBB", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(3));
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// 新增公告
        /// </summary>
        [HttpPost]
        public async Task CreateAnnouncementAsync([FromBody] AnnouncementCreateDto createDto)
        {
            Task.Delay(100).Wait();
        }

        /// <summary>
        /// 更新指定 ID 的公告 (完整的資源替換)
        /// </summary>
        [HttpPut("{id}")]
        public async Task UpdateAnnouncementAsync(int id, [FromBody] AnnouncementUpdateDto updateDto)
        {
            Task.Delay(100).Wait();
        }

        /// <summary>
        /// 刪除指定 ID 的公告
        /// </summary>
        [HttpDelete("{id}")]
        public async Task DeleteAnnouncementAsync(int id)
        {
            Task.Delay(100).Wait();
        }
    }

    public abstract class AnnouncementDto
    {
        public string Title { get; init; }
        public string? Content { get; init; }
        public DateTimeOffset StartAt { get; init; }
        public DateTimeOffset? EndAt { get; init; }
    }

    public class AnnouncementUpdateDto : AnnouncementDto
    {
    }

    public class AnnouncementCreateDto : AnnouncementDto
    {
        public int Id { get; init; }
    }

    public class AnnouncementsUserView
    {
        public AnnouncementsUserView(IEnumerable<Announcement> announcements)
        {
            Announcements = announcements;
        }

        public IEnumerable<Announcement> Announcements { get; init; }
    }
    public class Announcement
    {
        public int Id { get; set; }
        public required string Title { get; set; }
    }

    public class AnnouncementUserView : AnnouncementDto
    {
        public AnnouncementUserView(int id, string title, string content, DateTimeOffset startAt, DateTimeOffset? endAt)
        {
            Id = id;
            Title = title;
            Content = content;
            StartAt = startAt;
            EndAt = endAt;
        }

        public int Id { get; init; }
    }

    public sealed class GetSomethingParam
    {
        public sealed class Query
        {
            /// <summary>
            /// 標題
            /// </summary>
            public string? Title { get; set; }
            public DateTimeOffset? StartAt { get; set; }
            public DateTimeOffset? EndAt { get; set; }
        }
    }
}