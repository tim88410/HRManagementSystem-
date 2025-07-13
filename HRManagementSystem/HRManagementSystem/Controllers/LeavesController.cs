using DBUtility;
using HRManagementSystem.Application.Commands.Leaves;
using HRManagementSystem.Application.Queries.Leaves;
using HRManagementSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiResult]
    [APIError]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly IMediator mediator;
        public LeavesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// 依據Filter對Leave進行查詢，取得List資料
        /// </summary>
        /// <remarks>
        /// <code>
        /// <br/>
        /// 透過 ReturnCode 判斷狀態:<br/>
        /// ParamError(2) 查詢參數錯誤<br/>
        /// DBConnectError(3) 查詢入DB時連線失敗<br/>
        /// OperationSuccessful(5) 查詢成功<br/>
        /// DataNotFound(6) API呼叫成功，但沒有任何資料回傳<br/>
        /// </code>
        /// </remarks>
        [ApiLogging]
        [ApiResult]
        [APIError]
        [HttpGet]
        [Route("v1/Leaves")]
        public async Task<IActionResult> Get([FromQuery] LeavesRequest request)
        {
            if (!ModelState.IsValid)
            {
                throw new APIError.ParamError();
            }
            var result = await mediator.Send(request);

            if (result == null)
            {
                throw new APIError.DBConnectError();
            }
            else if (result.Total == 0)
            {
                throw new APIError.DataNotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// 傳入Leave 主Key [Id]進行查詢，取得單一資料，通常用於單一筆資料編輯時，Load的情況
        /// </summary>
        /// <remarks>
        /// <code>
        /// <br/>
        /// 透過 ReturnCode 判斷狀態:<br/>
        /// ParamError(2) 查詢參數錯誤<br/>
        /// DBConnectError(3) 查詢入DB時連線失敗<br/>
        /// OperationSuccessful(5) 查詢成功<br/>
        /// DataNotFound(6) API呼叫成功，但沒有任何資料回傳<br/>
        /// </code>
        /// </remarks>
        [ApiLogging]
        [ApiResult]
        [APIError]
        [HttpGet]
        [Route("v1/Leaves/{Id}")]
        public async Task<IActionResult> GetOne(int Id)
        {
            if (!ModelState.IsValid)
            {
                throw new APIError.ParamError();
            }
            var result = await mediator.Send(new LeavesGetOneRequest { Id = Id });

            if (result == null)
            {
                throw new APIError.DBConnectError();
            }
            else if (!result.Any())
            {
                throw new APIError.DataNotFound();
            }
            return Ok(result.FirstOrDefault());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <code>
        /// Id:流水號，供編輯時選定單筆Id
        /// <br/>
        /// 透過 ReturnCode 判斷狀態:<br/>
        /// ParamError(2) 參數錯誤<br/>
        /// DBConnectError(3) DB連線失敗<br/>
        /// OperationSuccessful(5) 查詢成功<br/>
        /// </code>
        /// </remarks>
        [ApiLogging]
        [ApiResult]
        [APIError]
        [HttpPost]
        [Route("v1/Leaves")]
        public async Task<IActionResult> Post([FromBody] UpdateLeaveCommand request)
        {
            if (!ModelState.IsValid || request.Id > 0)
            {
                throw new APIError.ParamError();
            }
            var result = await mediator.Send(request);

            if (result == ErrorCode.KErrDBError)
            {
                throw new APIError.DBConnectError();
            }
            return Ok();
        }

        /// <summary>
        /// 針對某筆資料進行修改，須注意修改時Id必不得為0，若Id為0則判定為新增，會回傳參數錯誤
        /// </summary>
        /// <remarks>
        /// <code>
        /// Id:流水號，供編輯時選定單筆Id
        /// <br/>
        /// 透過 ReturnCode 判斷狀態:<br/>
        /// ParamError(2) 參數錯誤<br/>
        /// DBConnectError(3) DB連線失敗<br/>
        /// OperationSuccessful(5) 修改成功<br/>
        /// </code>
        /// </remarks>
        [ApiLogging]
        [ApiResult]
        [APIError]
        [HttpPut]
        [Route("v1/Leaves")]
        public async Task<IActionResult> Put([FromBody] UpdateLeaveCommand request)
        {
            if (!ModelState.IsValid || request.Id == 0)
            {
                throw new APIError.ParamError();
            }
            var result = await mediator.Send(request);

            if (result == (int)ErrorCode.ReturnCode.DataNotFound)
            {
                throw new APIError.DataNotFound();
            }
            if (result == ErrorCode.KErrDBError)
            {
                throw new APIError.DBConnectError();
            }
            return Ok();
        }

        /// <summary>
        /// 刪除Leaves內某筆資料，須注意刪除時Id必不得為0，若Id為0則判定為參數錯誤
        /// </summary>
        /// <remarks>
        /// <code>
        /// <br/>
        /// 透過 ReturnCode 判斷狀態:<br/>
        /// ParamError(2) 參數錯誤<br/>
        /// DBConnectError(3) DB連線失敗<br/>
        /// OperationSuccessful(5) 刪除成功<br/>
        /// </code>
        /// </remarks>
        [ApiLogging]
        [ApiResult]
        [APIError]
        [HttpDelete]
        [Route("v1/Leaves")]
        public async Task<IActionResult> Delete(int Id, int userId)
        {
            if (!ModelState.IsValid || Id == 0)
            {
                throw new APIError.ParamError();
            }
            var result = await mediator.Send(new DeleteLeaveCommand { Id = Id , UserId = userId });

            if (result == ErrorCode.KErrDBError)
            {
                throw new APIError.DBConnectError();
            }
            return Ok();
        }
    }
}
