﻿namespace HandFootExcluded;

public sealed class NewGameEvent
{
    public static readonly NewGameEvent Instance = new();

    private NewGameEvent() {}
}