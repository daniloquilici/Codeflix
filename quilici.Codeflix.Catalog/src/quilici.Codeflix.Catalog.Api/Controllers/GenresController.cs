using MediatR;
using Microsoft.AspNetCore.Mvc;
using quilici.Codeflix.Catalog.Api.ApiModels.Genre;
using quilici.Codeflix.Catalog.Api.ApiModels.Response;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.GetGenre;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre;

namespace quilici.Codeflix.Catalog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GenresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<GenreModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(new GetGenreInput(id), cancellationToken);
            return Ok(new ApiResponse<GenreModelOutput>(output));
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<GenreModelOutput>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteGenreInput(id), cancellationToken);
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GenreModelOutput>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CreateGenreInput input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = output.Id }, new ApiResponse<GenreModelOutput>(output));
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<GenreModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromBody] UpdateGenreApiInput input, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(new UpdateGenreInput(id, input.Name, input.IsActive, input.CategoriesIds), cancellationToken);
            return Ok(new ApiResponse<GenreModelOutput>(output));
        }
    }
}
