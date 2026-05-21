namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;

/// <summary>
/// Tests for <see cref="DefaultEnumMultiSelectHandler{T,TEnum}"/> which handles
/// filtering on entity properties that are collections of enum values.
/// </summary>
public class MuiDataGridEnumMultiSelectTests
{
    private enum Role
    {
        Admin,
        SuperUser,
        ReadOnly,
        Viewer,
    }

    private enum Permission
    {
        Read,
        Write,
    }

    private static List<UserWithRoles> TestUsers =>
    [
        new() { Id = 1, Name = "Alice", Roles = [Role.Admin] },
        new() { Id = 2, Name = "Bob", Roles = [Role.SuperUser] },
        new() { Id = 3, Name = "Carol", Roles = [Role.Admin, Role.SuperUser] },
        new() { Id = 4, Name = "Dave", Roles = [Role.ReadOnly] },
        new() { Id = 5, Name = "Eve", Roles = [] },
    ];

    [Fact]
    public void IsAnyOf_SingleFilter_ReturnsUsersWithMatchingRole()
    {
        var dataType = new UserWithRolesDataType();
        var filter = new MuiDataGridFilterInput
        {
            Items =
            [
                new(
                    Field: "roles",
                    Value: new MuiValue(new List<string> { "ADMIN" }),
                    Operator: "isAnyOf"),
            ],
        };

        var result = TestUsers.AsQueryable().Where(dataType.Filter(filter)).ToList();

        result.Should().HaveCount(2);
        result.Select(u => u.Name).Should().BeEquivalentTo("Alice", "Carol");
    }

    [Fact]
    public void IsAnyOf_MultipleFilterValues_ReturnsUsersWithAnyMatchingRole()
    {
        var dataType = new UserWithRolesDataType();
        var filter = new MuiDataGridFilterInput
        {
            Items =
            [
                new(
                    Field: "roles",
                    Value: new MuiValue(new List<string> { "ADMIN", "SUPER_USER" }),
                    Operator: "isAnyOf"),
            ],
        };

        var result = TestUsers.AsQueryable().Where(dataType.Filter(filter)).ToList();

        // Alice (Admin), Bob (SuperUser), Carol (Admin + SuperUser) — Dave (ReadOnly) and Eve (empty) excluded
        result.Should().HaveCount(3);
        result.Select(u => u.Name).Should().BeEquivalentTo("Alice", "Bob", "Carol");
    }

    [Fact]
    public void IsAnyOf_NoOverlap_ReturnsEmptyList()
    {
        var dataType = new UserWithRolesDataType();
        var filter = new MuiDataGridFilterInput
        {
            Items =
            [
                new(
                    Field: "roles",
                    Value: new MuiValue(new List<string> { "VIEWER" }),
                    Operator: "isAnyOf"),
            ],
        };

        var result = TestUsers.AsQueryable().Where(dataType.Filter(filter)).ToList();

        result.Should().BeEmpty();
    }

    [Fact]
    public void IsAnyOf_AllRolesInFilter_ReturnsAllUsersWithAtLeastOneRole()
    {
        var dataType = new UserWithRolesDataType();
        var filter = new MuiDataGridFilterInput
        {
            Items =
            [
                new(
                    Field: "roles",
                    Value: new MuiValue(new List<string> { "ADMIN", "SUPER_USER", "READ_ONLY", "VIEWER" }),
                    Operator: "isAnyOf"),
            ],
        };

        var result = TestUsers.AsQueryable().Where(dataType.Filter(filter)).ToList();

        // Eve has no roles and should be excluded
        result.Should().HaveCount(4);
        result.Select(u => u.Name).Should().BeEquivalentTo("Alice", "Bob", "Carol", "Dave");
    }

    [Fact]
    public void IsAnyOf_UserWithMultipleRoles_OnlyMatchesWhenFilterRolePresent()
    {
        var dataType = new UserWithRolesDataType();
        var filter = new MuiDataGridFilterInput
        {
            Items =
            [
                new(
                    Field: "roles",
                    Value: new MuiValue(new List<string> { "READ_ONLY" }),
                    Operator: "isAnyOf"),
            ],
        };

        var result = TestUsers.AsQueryable().Where(dataType.Filter(filter)).ToList();

        result.Should().HaveCount(1);
        result.Single().Name.Should().Be("Dave");
    }

    [Theory]
    [InlineData("ADMIN")]
    [InlineData("Admin")]
    [InlineData("admin")]
    public void IsAnyOf_EnumNameCaseVariants_ParsedCorrectly(string enumName)
    {
        var dataType = new UserWithRolesDataType();
        var filter = new MuiDataGridFilterInput
        {
            Items =
            [
                new(
                    Field: "roles",
                    Value: new MuiValue(new List<string> { enumName }),
                    Operator: "isAnyOf"),
            ],
        };

        var result = TestUsers.AsQueryable().Where(dataType.Filter(filter)).ToList();

        result.Should().HaveCount(2, $"'{enumName}' should resolve to Role.Admin");
        result.Select(u => u.Name).Should().BeEquivalentTo("Alice", "Carol");
    }

    [Fact]
    public void IsAnyOf_UnknownEnumValue_ThrowsArgumentException()
    {
        var dataType = new UserWithRolesDataType();
        var filter = new MuiDataGridFilterInput
        {
            Items =
            [
                new(
                    Field: "roles",
                    Value: new MuiValue(new List<string> { "GOD_MODE" }),
                    Operator: "isAnyOf"),
            ],
        };

        var act = () => TestUsers.AsQueryable().Where(dataType.Filter(filter)).ToList();

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Failed to Parse*");
    }

    [Fact]
    public void UnsupportedOperator_ThrowsArgumentException()
    {
        var dataType = new UserWithRolesDataType();
        var filter = new MuiDataGridFilterInput
        {
            Items =
            [
                new(
                    Field: "roles",
                    Value: new MuiValue(new List<string> { "ADMIN" }),
                    Operator: "is"), // only isAnyOf is supported
            ],
        };

        var act = () => TestUsers.AsQueryable().Where(dataType.Filter(filter)).ToList();

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Unknown operator*");
    }

    [Fact]
    public void AllCollectionInterfaceOverloads_RegisterWithoutError()
    {
        // Ensures all four overloads compile and register without exceptions at runtime.
        var act = () => new TestAllInterfacesDataType();

        act.Should().NotThrow();
    }

    private sealed class UserWithRolesDataType : DataType<UserWithRoles>
    {
        protected override void Configure(DataTypeBuilder<UserWithRoles> builder)
        {
            builder.Property(u => u.Name);
            builder.Property(u => u.Roles).SetEnumMultiSelectHandler();
        }
    }

    private sealed class TestAllInterfacesDataType : DataType<ResourceWithPermissions>
    {
        protected override void Configure(DataTypeBuilder<ResourceWithPermissions> builder)
        {
            builder.Property(r => r.RequiredPermissions).SetEnumMultiSelectHandler(); // ICollection<TEnum>
            builder.Property(r => r.GrantedPermissions).SetEnumMultiSelectHandler(); // IEnumerable<TEnum>
            builder.Property(r => r.DeniedPermissions).SetEnumMultiSelectHandler(); // List<TEnum>
        }
    }

    private class UserWithRoles
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public IList<Role> Roles { get; set; } = new List<Role>();
    }

    private class ResourceWithPermissions
    {
        public ICollection<Permission> RequiredPermissions { get; set; } = new List<Permission>();

        public IEnumerable<Permission> GrantedPermissions { get; set; } = new List<Permission>();

        public List<Permission> DeniedPermissions { get; set; } = [];
    }
}