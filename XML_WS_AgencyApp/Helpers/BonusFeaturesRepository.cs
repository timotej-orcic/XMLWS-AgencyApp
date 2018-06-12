using System.Collections.Generic;
using System.Linq;
using XML_WS_AgencyApp.Models;

namespace XML_WS_AgencyApp.Helpers
{
    public class BonusFeaturesRepository
    {
        public List<BonusFeaturesViewModel> GetBonusFeatures()
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<BonusFeatures> bonusFeatures = ctx.BonusFeatures.AsNoTracking()
                     .OrderBy(x => x.Name)
                        .ToList();

                List<BonusFeaturesViewModel> retList = new List<BonusFeaturesViewModel>();
                foreach(var b in bonusFeatures)
                {
                    retList.Add(
                        new BonusFeaturesViewModel
                        {
                            Id = b.Id,
                            Name = b.Name,
                            IsSelected = false
                        }
                    );
                }

                return retList;
            }
        }
    }
}