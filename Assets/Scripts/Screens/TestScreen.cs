using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -> Prepare ScreenManager in the main scene
// -> Test
// -> Make ScreenManager a true singleton

public class TestScreen : GenericScreen {

    public override void Open() {
        FocusCamera();
    }

    public override void Close() {

    }

}
