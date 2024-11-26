using System;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entites;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAuctions()
    {
        var auctions = await _context.Auctions
        .Include(a => a.Item)
        .OrderBy(a => a.Item.Make)
        .ToListAsync();

        var auctionsDto = _mapper.Map<List<AuctionDto>>(auctions);

        return Ok(auctionsDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetById(Guid id)
    {
        var auction = await _context.Auctions
        .Include(a => a.Item)
        .FirstOrDefaultAsync(a => a.Id == id);

        if (auction == null)
        {
            return NotFound();
        }

        var auctionsDto = _mapper.Map<AuctionDto>(auction);

        return Ok(auctionsDto);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        var auction = _mapper.Map<Auction>(createAuctionDto);
        auction.Seller = "test";
        _context.Auctions.Add(auction);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest();

        var auctionDto = _mapper.Map<AuctionDto>(auction);

        return CreatedAtAction(nameof(GetById), new { id = auction.Id }, auctionDto);
    }
}
