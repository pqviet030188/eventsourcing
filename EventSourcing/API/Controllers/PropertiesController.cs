using Marten;
using Microsoft.AspNetCore.Mvc;
using Demo.Models.Domains;
using Demo.Models.Events.Property;
using MediatR;
using Demo.Models;
//using Demo.Services.Handler;

namespace Demo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IDocumentSession _session;
    private readonly IMediator _mediator;
    public PropertiesController(IDocumentSession session, IMediator mediator)
    {
        _session = session;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Property property)
    {
        _session.Store(property);
        await _session.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = property.Id }, property);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Property update)
    {
        var property = await _session.LoadAsync<Property>(id);
        if (property == null) return NotFound();

        property.Update(update.Address, update.OwnerName, update.Price);
        _session.Store(property);
        await _session.SaveChangesAsync();
        return Ok(property);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var property = await _session.LoadAsync<Property>(id);
        return property == null ? NotFound() : Ok(property);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        //var properties = await _session.Query<Property>().ToListAsync();
        //return Ok(properties);


        var propertyId = Guid.NewGuid();

        _session.Events.StartStream<PropertyCreate>(propertyId, new PropertyCreate(propertyId, "123 Main St", 100000m));

        await _session.SaveChangesAsync();
        // await _mediator.Publish(new EventNotification<PropertyCreate>(new PropertyCreate(Guid.NewGuid(), "", 22)));
        //var a = new A()
        //{
        //    name  = "rex"
        //};
        //await _mediator.Send(a);
        return Ok(new { });
    }
}