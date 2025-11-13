using Marten;
using Microsoft.AspNetCore.Mvc;
using Demo.Models.Domains;
using Demo.Models.Events.Property;
using MediatR;
using Demo.Models;
using Demo.EventStore.Events.Property;
using ImTools;
//using Demo.Services.Handler;

namespace Demo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IDocumentSession _session;
    public PropertiesController(IDocumentSession session)
    {
        _session = session;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Property property)
    {
        var propertyId = Guid.NewGuid();
        property.Id = propertyId;
        var createEvent = new PropertyCreate(property!.Id, property!.Address, property!.Price);
        _session.Events.StartStream<PropertyCreate>(createEvent.PropertyId, createEvent);
        await _session.SaveChangesAsync();
        return Ok(createEvent);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, Property update)
    {
        
        var updateEvent = new PropertyUpdate(id, update!.Address, update!.Price);
        _session.Events.StartStream<PropertyUpdate>(updateEvent.PropertyId, updateEvent);

        await _session.SaveChangesAsync();
        return Ok(updateEvent);
    }
}