# Multi-frustum
 Test multiple display outputs with frustum inputs or VIOSO API view calculation.

 # How to use
- [Download the executable build from the releases](https://github.com/AJvioso/Multi-Frustum/releases)
- Run on the target machine(s). All windows displays are activated, and a camera is assigned to each.
- **Manual Mode:**
  You can edit the FOV an direction values in the UI.
    - FOV: Angles in degrees: left, right, top, bottom.
    - Direction: Angles in degrees: yaw turns right, positive pitch is up and positive roll turns clockwise.

- **VIOSO mode:**
  Configure the VIOSOWarpBlend.ini file. You can either open it from the UI button or from this path `\MultiFrustum_Data\Plugins\x86_64`
> **Note:** Make sure you restart the scene (Button ⟳) every time you edit the `.ini` file to view the changes.

# Reference
- VIOSO 6 software: https://helpdesk.vioso.com/documentation/vioso-6-overview/
- VIOSO Unity plugin: https://helpdesk.vioso.com/documentation/integrate-3d-engines/unity/
- Ini file reference: https://helpdesk.vioso.com/documentation/api/viosowarpblend-ini-reference/

