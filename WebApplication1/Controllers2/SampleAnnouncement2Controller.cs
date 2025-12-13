using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers2
{
    // ------------------------------------------------------------------
    // I. 資料模型 (DTOs / View Models)
    // ------------------------------------------------------------------

    // 基礎輸入模型 (使用 record 類型，簡潔且適合 DTO)
    public record AnnouncementInputDto(
        string Title,
        string Content,
        DateTimeOffset StartAt,
        DateTimeOffset? EndAt
    );

    // 新增模型 (不需要 ID)
    public record AnnouncementCreateDto2(
        string Title,
        string Content,
        DateTimeOffset StartAt,
        DateTimeOffset? EndAt
    ) : AnnouncementInputDto(Title, Content, StartAt, EndAt);

    // 更新模型 (需要 ID)
    public record AnnouncementUpdateDto2(
        int Id,
        string Title,
        string Content,
        DateTimeOffset StartAt,
        DateTimeOffset? EndAt
    ) : AnnouncementInputDto(Title, Content, StartAt, EndAt);

    // 公告列表摘要 (List Item View)
    public record AnnouncementListItemView(
        int Id,
        string Title
    );

    // 公告詳細檢視 (Full View)
    public record AnnouncementUserView2(
        int Id,
        string Title,
        string Content,
        DateTimeOffset StartAt,
        DateTimeOffset? EndAt
    );

    // 公告列表容器
    public record AnnouncementsUserView2(
        IEnumerable<AnnouncementListItemView> Announcements
    );


    // ------------------------------------------------------------------
    // II. 核心 Controller 實作
    // ------------------------------------------------------------------

    [ApiController]
    [Route("api/[controller]")] // 加上 api/ 前綴是常見的 RESTful 慣例
    public class SampleAnnouncement2Controller : ControllerBase
    {
        // 模擬資料庫儲存 (直接放在 Controller 中)
        private static readonly List<AnnouncementUserView2> _announcements = new List<AnnouncementUserView2>
        {
            new (1, "第一則公告", "這是第一則公告的詳細內容。", DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddHours(3)),
            new (2, "第二則公告", "這是第二則公告的詳細內容。", DateTimeOffset.UtcNow.AddDays(-2), null)
        };
        private static int _nextId = 3;

        public SampleAnnouncement2Controller()
        {
            // 在實際應用中，這裡會注入 Service 或 Repository
        }

        // --- C (Create) ---
        /// <summary>
        /// 新增公告
        /// HTTP POST /api/SampleAnnouncement
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AnnouncementUserView2))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAnnouncementAsync([FromBody] AnnouncementCreateDto2 createDto)
        {
            // 替換 Task.Delay(100).Wait() 為正確的非同步等待
            await Task.Delay(100);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad Request
            }

            // 模擬新增邏輯
            var newAnnouncement = new AnnouncementUserView2(
                _nextId++,
                createDto.Title,
                createDto.Content,
                createDto.StartAt,
                createDto.EndAt
            );
            _announcements.Add(newAnnouncement);

            // 201 Created，Location Header 指向新資源的 GET 路由
            return CreatedAtAction(
                nameof(GetAnnouncementByIdAsync),
                new { id = newAnnouncement.Id },
                newAnnouncement);
        }

        // --- R (Read All) ---
        /// <summary>
        /// 取得所有公告列表
        /// HTTP GET /api/SampleAnnouncement
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnnouncementsUserView2))]
        public async Task<IActionResult> GetAnnouncementsAsync()
        {
            await Task.Delay(100);

            // 轉換為列表摘要 View Model
            var listItems = _announcements.Select(a => new AnnouncementListItemView(a.Id, a.Title));
            var result = new AnnouncementsUserView2(listItems.OrderByDescending(a => a.Id));

            return Ok(result); // 200 OK
        }

        // --- R (Read Single) ---
        /// <summary>
        /// 根據 ID 取得單一公告
        /// HTTP GET /api/SampleAnnouncement/{id}
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnnouncementUserView2))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAnnouncementByIdAsync(int id)
        {
            await Task.Delay(100);

            // 模擬查詢邏輯
            var announcement = _announcements.FirstOrDefault(a => a.Id == id);

            if (announcement == null)
            {
                return NotFound($"找不到 ID 為 {id} 的公告。"); // 404 Not Found
            }

            return Ok(announcement); // 200 OK
        }

        // --- U (Update) ---
        /// <summary>
        /// 更新指定 ID 的公告 (完整的資源替換)
        /// HTTP PUT /api/SampleAnnouncement/{id}
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAnnouncementAsync(int id, [FromBody] AnnouncementUpdateDto2 updateDto)
        {
            await Task.Delay(100);

            // 驗證 ID 是否一致
            if (id != updateDto.Id)
            {
                return BadRequest("路由 ID 與請求內容 ID 不一致。"); // 400 Bad Request
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad Request
            }

            // 模擬更新邏輯
            var index = _announcements.FindIndex(a => a.Id == id);

            if (index == -1)
            {
                return NotFound($"找不到 ID 為 {id} 的公告，無法更新。"); // 404 Not Found
            }

            var updatedAnn = new AnnouncementUserView2(
                updateDto.Id,
                updateDto.Title,
                updateDto.Content,
                updateDto.StartAt,
                updateDto.EndAt
            );

            _announcements[index] = updatedAnn;

            return NoContent(); // 204 No Content (表示成功但沒有內容回傳)
        }

        // --- D (Delete) ---
        /// <summary>
        /// 刪除指定 ID 的公告
        /// HTTP DELETE /api/SampleAnnouncement/{id}
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAnnouncementAsync(int id)
        {
            await Task.Delay(100);

            // 模擬刪除邏輯
            var countBefore = _announcements.Count;
            _announcements.RemoveAll(a => a.Id == id);
            var isDeleted = _announcements.Count < countBefore;

            if (!isDeleted)
            {
                return NotFound($"找不到 ID 為 {id} 的公告，無法刪除。"); // 404 Not Found
            }

            return NoContent(); // 204 No Content
        }
    }
}