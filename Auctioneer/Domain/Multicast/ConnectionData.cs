﻿namespace Domain.Criptography
{
    public record ConnectionData(string MultiCastAddress, int Port, SymmetricKey SymmetricKey);
}
