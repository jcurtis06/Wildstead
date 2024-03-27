using Godot;
using System;

public partial class CraftingMenu : PanelContainer
{
    [Export] public Crafter Crafter;
    [Export] public PackedScene RecipeItem;
    [Export] public Inventory ViewerInventory;

    private VBoxContainer _recipeContainer;

    public override void _Ready()
    {
        _recipeContainer = GetNode<VBoxContainer>("CraftingContainer/RecipeContainer");
        UpdateRecipes();
    }
    
    public void UpdateRecipes()
    {
        foreach (var recipe in Crafter.Recipes)
        {
            if (recipe == null)
                continue;
            
            var recipeItem = RecipeItem.Instantiate() as RecipeDisplay;
            recipeItem?.SetRecipe(recipe);
            _recipeContainer.AddChild(recipeItem);
        }
    }
    
    public void HandleRecipeClick(Recipe recipe)
    {
        if (ViewerInventory == null)
            return;

        var canCraft = true;
        
        foreach (var ingredient in recipe.Ingredients)
        {
            if (ingredient == null)
                continue;
            
            if (!ViewerInventory.HasItem(ingredient))
                return;
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            if (ingredient == null)
                continue;
            
            ViewerInventory.RemoveItem(ingredient, ingredient.Count);
        }
        
        GD.Print("Crafting item: ", recipe.Result.Name);
        ViewerInventory.AddItem(recipe.Result.Duplicate() as Item);
    }
}
