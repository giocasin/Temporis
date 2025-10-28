namespace Temporis.Core.DTOs;

public record LocalDateTimeDto(int Year, int Month, int Day, int Hour, int Minute, int Second)
{
    public override string ToString() => $"{Year:0000}-{Month:00}-{Day:00} {Hour:00}:{Minute:00}:{Second:00}";
}