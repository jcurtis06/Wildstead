using Godot;
using System;

public partial class RecipeDisplay : Button
{
    [Export] public Recipe Recipe;
    
    private CraftingMenu _craftingMenu;
    
    public override void _Ready()
    {
        _craftingMenu = GetNode<CraftingMenu>("../../../../CraftingMenu");
    }
    
    public void SetRecipe(Recipe recipe)
    {
        Recipe = recipe;
        Text = recipe.Result.Name;
    }
    
    public void OnRecipePressed()
    {
        _craftingMenu.HandleRecipeClick(Recipe);
    }
}
