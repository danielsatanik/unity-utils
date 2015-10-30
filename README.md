# Unity3d Utils by Daniel Satanik

## Version 0.0.002

### NEW

#### Extensions
* Exists (test for null but positive formulated)

### BUGFIXES

#### Custom Property Types
* SceneSelector -- did not save selection

## Version 0.0.001

### NEW

#### Attributes
* Prefab (passes prefab for property drawer or custom editor)
* ReadOnly (makes property readonly in play mode)

#### Custom Inspector Types
* PercentSplitter (allows defining key-value-pairs with percent and type)
* SceneSelector (let one select any available scene in build settings)

#### CustomEditor
* Transform (adds NGUI reset buttons, when not using NGUI)

#### PropertyDrawer
* Flag (typical flags popup)
* TagSelector (gives a list of all available tags to select from)

#### Utilites
* ScriptableObjectUtility (wrapper for instantiating scriptable object)

### Behaviours
* SingletonBehaviour (wraps singleton functionality)

#### Extensions
* .In/.NotIn (used for tests in or not in arrays)
* IList - .AddMultiple and .Shuffle
* Stream - .CopyTo

#### Components
* Logger (Logs to console, asserts, writes to files, rotates files)
* Spatial UI (uGui Spatial UI stays relative to camera)