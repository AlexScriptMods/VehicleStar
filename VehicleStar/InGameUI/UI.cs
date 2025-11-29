using GTA;
using GTA.Native;
using LemonUI;
using LemonUI.Menus;
using System.Collections.Generic;

public class UI
{
    private ObjectPool pool = new ObjectPool();
    private Stack<NativeMenu> menuStack = new Stack<NativeMenu>();

    public NativeMenu mainMenu { get; private set; }
    public NativeMenu yvrMenu { get; private set; }

    public UI()
    {
        mainMenu = MenuMain.Create(this);
        yvrMenu = MenuYVRPlayback.Create(this);

        pool.Add(mainMenu);
        pool.Add(yvrMenu);

        mainMenu.Visible = false;
        yvrMenu.Visible = false;
    }

    public void OpenMenu(NativeMenu menu)
    {
        if (menuStack.Count > 0)
            menuStack.Peek().Visible = false;

        menu.Visible = true;
        menuStack.Push(menu);
    }

    public void CloseCurrentMenu()
    {
        if (menuStack.Count == 0) return;

        var top = menuStack.Pop();
        top.Visible = false;

        if (menuStack.Count > 0)
        {
            var previous = menuStack.Peek();
            previous.Visible = true;
        }
    }

    public void ToggleMenu()
    {
        if (menuStack.Count == 0)
            OpenMenu(mainMenu);
        else
            CloseCurrentMenu();
    }

    public void Update()
    {
        pool.Process();
        HandleMouseScroll();

        Function.Call(Hash.SET_MOUSE_CURSOR_VISIBLE, false);

    }

    private void HandleMouseScroll()
    {
        if (menuStack.Count == 0) return;

        var active = menuStack.Peek();
        int selected = active.SelectedIndex;

        int scroll = Game.GetControlValueNormalized(GTA.Control.CursorScrollUp) > 0 ? -1 :
                     Game.GetControlValueNormalized(GTA.Control.CursorScrollDown) > 0 ? 1 : 0;

        if (scroll != 0)
        {
            selected += scroll;

            if (selected < 0)
                selected = active.Items.Count - 1;
            else if (selected >= active.Items.Count)
                selected = 0;

            active.SelectedIndex = selected;
        }
    }
}