using GTA;
using LemonUI.Menus;
using System.Windows.Forms;
using VehicleStar;

public class MenuYVRPlayback
{
    private static string lastFileName = "";
    private static string lastFileIndex = "";
    private static bool lastShouldWarpIntoVehicle = true;

    public static string selectedFileName { get; private set; }
    public static string selectedFileIndex { get; private set; }
    public static bool shouldWarpIntoVehicle { get; private set; } = false;

    public static NativeMenu Create(UI ui)
    {
        var menu = new NativeMenu("VehicleStar", "YVR Playback");

        var name = new NativeItem("File Name");
        var index = new NativeItem("File Index");
        var shouldWarp = new NativeCheckboxItem("Warp Into vehicle");
        var play = new NativeItem("Play");
        var backItem = new NativeItem("Back");

        //Set previous values
        name.Title = "Name: " + lastFileName;
        index.Title = "Index: " + lastFileIndex;
        shouldWarp.Checked = shouldWarpIntoVehicle;

        name.Activated += (m, i) =>
        {
            string input = Game.GetUserInput();

            if (!string.IsNullOrEmpty(input))
            {
                name.Title = "Name: " + input;
                selectedFileName = input;
                lastFileName = input;
            }

        };

        index.Activated += (m, i) =>
        {
            string input = Game.GetUserInput();

            if (!string.IsNullOrEmpty(input))
            {
                index.Title = "Index: " + input;
                selectedFileIndex = input;
                lastFileIndex = input;
            }

        };

        shouldWarp.Activated += (m, i) =>
        {
            shouldWarpIntoVehicle = shouldWarp.Checked;
            lastShouldWarpIntoVehicle = shouldWarp.Checked;
        };

        play.Activated += (m, i) =>
        {
            Main.yvrPlayback.PlayRecording(selectedFileIndex, selectedFileName, shouldWarpIntoVehicle);
        };

        backItem.Activated += (m, i) => ui.CloseCurrentMenu();

        menu.Add(name);
        menu.Add(index);
        menu.Add(shouldWarp);
        menu.Add(play);
        menu.Add(backItem);

        return menu;
    }
}
