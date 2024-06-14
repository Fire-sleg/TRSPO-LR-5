using AutoMapper;
using Azure;
using BasketAPI.Models;
using BasketAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Net;
using StackExchange.Redis;
using BasketAPI.Redis.IRepository;

namespace BasketAPI.Controllers
{
    [Route("api/v{version:apiVersion}/Basket")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class BasketController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IRepository _dbProduct;
        private readonly IMapper _mapper;

        public BasketController(IRepository dbProduct, IMapper mapper)
        {
            _dbProduct = dbProduct;
            _mapper = mapper;
            _response = new();


        }

        //[Authorize]
        [HttpGet("Products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProductsAsync()
        {
            try
            {
                IEnumerable<Product> productList = await _dbProduct.GetAllAsync();
                _response.Result = _mapper.Map<List<ProductDTO>>(productList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorsMessages.Add(ex.ToString());
            }
            return StatusCode(StatusCodes.Status500InternalServerError, _response);
        }

        //[Authorize]
        [MapToApiVersion("2.0")]
        [HttpGet("GetProduct/{id:Guid}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var product = await _dbProduct.GetAsync(id);
                _response.Result = _mapper.Map<ProductDTO>(product);
                _response.StatusCode = HttpStatusCode.OK;
                if (product != null)
                {
                    return Ok(_response);
                }
                else
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorsMessages.Add(ex.ToString());
            }
            return StatusCode(StatusCodes.Status500InternalServerError, _response);

        }
        [MapToApiVersion("2.0")]
        [HttpPost("CreateProduct/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateProductAsync([FromBody] ProductDTO createDTO) //ProductCreateDTO
        {
            try
            {
                
                if (createDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorsMessages.Add("Product is null");
                    return BadRequest(_response);
                }
                List<Product> list = await _dbProduct.GetAllAsync();
                if (list.Any(obj => obj.Name == createDTO.Name))  //await _dbProduct.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorsMessages.Add("Product is already exists!");
                    return BadRequest(_response);
                }

                Product model = _mapper.Map<Product>(createDTO);

                await _dbProduct.CreateAsync(model);

                _response.IsSuccess = true;
                _response.Result = _mapper.Map<ProductDTO>(model); ;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetProduct", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorsMessages.Add(ex.ToString());
            }
            return StatusCode(StatusCodes.Status500InternalServerError, _response);

        }
        [MapToApiVersion("2.0")]
        [HttpDelete("DeleteProduct/{id:Guid}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteProductAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                var product = await _dbProduct.GetAsync(id);
                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                await _dbProduct.RemoveAsync(product);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return NoContent();//Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorsMessages.Add(ex.ToString());
            }
            return StatusCode(StatusCodes.Status500InternalServerError, _response);

        }



    }
}
