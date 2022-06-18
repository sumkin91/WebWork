﻿namespace WebWork.Intefaces.TestApi;

public interface IValuesService
{
    IEnumerable<string> GetValues();

    string? GetById(int Id);

    void Add(string Value);

    void Edit(int Id, string Value);

    bool Delete(int Id);
}
