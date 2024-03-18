using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerpNES;

public interface ICpu
{
    void ConnectBus( IBus bus );
}
