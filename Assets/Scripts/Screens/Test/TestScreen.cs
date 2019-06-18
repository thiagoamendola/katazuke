using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -> Make ScreenManager a true singleton

public class TestScreen : GenericScreen {

    public override void Open() {
        StartCoroutine(AsyncOpen());
    }

    IEnumerator AsyncOpen(){
        yield return FocusCamera();
        ScreenManager.GoToTitleScreen();
    }

    public override void Close() {

    }

}
