public class AIController : Controller {
    public override void Initialize (Unit owner) { base.Initialize (owner); this.Brain = BrainFactory.CreateBrainFromUnit (owner); }
    public override bool DetectInputFor (ControlTypes type) { return true; }
}