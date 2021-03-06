# FluentValidation.Blazor

Add FluentValidation to your Blazor forms

### Getting Started
Create validators for your form model

Register validatiors and required services
```csharp
services.AddFluentValidation(typeof(Startup));
```

Tag your form to add validation messages
```
<EditForm Model="User" OnValidSubmit="OnValidSubmit">
    <FluentValidationValidator />
    <FormBlock>
        <label>First Name</label>
        <InputText @bind-Value="User.FirstName" />
        <ValidationMessage For="() => User.FirstName" />
    </FormBlock>
    <FormBlock>
        <label>Last Name</label>
        <InputText @bind-Value="User.LastName" />
        <ValidationMessage For="() => User.LastName" />
    </FormBlock>
    <FormBlock>
        <button type="submit" class="btn btn-primary">Submit</button>
    </FormBlock>
</EditForm>
```

### Contributing
Want to contribute? Great!

### License
[MIT](https://github.com/kevinarthurackerman/FluentValidation.Blazor/blob/master/LICENSE.md)
