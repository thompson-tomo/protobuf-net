// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: TypeRefs.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace TypeRefs
{

    [global::ProtoBuf.ProtoContract(Name = @"msg_a")]
    public partial class MsgA : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"a")]
        public global::TypeRefs.MsgA A { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"b")]
        public global::TypeRefs.MsgB B { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"c")]
        public global::TypeRefs.MsgB.MsgC C { get; set; }

        [global::ProtoBuf.ProtoMember(4, Name = @"d")]
        public global::TypeRefs.MsgD D { get; set; }

        internal static void Serialize(MsgA value, ref global::ProtoBuf.Nano.Writer writer)
        {
            if (value.A is global::TypeRefs.MsgA obj1)
            {
                writer.WriteVarint(10); // field 1, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgA.Measure(obj1);
                global::TypeRefs.MsgA.Write(obj1, ref writer);
            }
            if (value.B is global::TypeRefs.MsgB obj2)
            {
                writer.WriteVarint(18); // field 2, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgB.Measure(obj2);
                global::TypeRefs.MsgB.Write(obj2, ref writer);
            }
            if (value.C is global::TypeRefs.MsgB.MsgC obj3)
            {
                writer.WriteVarint(26); // field 3, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgB.MsgC.Measure(obj3);
                global::TypeRefs.MsgB.MsgC.Write(obj3, ref writer);
            }
            if (value.D is global::TypeRefs.MsgD obj4)
            {
                writer.WriteVarint(34); // field 4, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgD.Measure(obj4);
                global::TypeRefs.MsgD.Write(obj4, ref writer);
            }
        }

        internal static ulong Measure(MsgA value)
        {
            ulong len = 0;
            if (value.A is global::TypeRefs.MsgA obj1)
            {
                len += 1 + global::TypeRefs.MsgA.Measure(obj1);
            }
            if (value.B is global::TypeRefs.MsgB obj2)
            {
                len += 1 + global::TypeRefs.MsgB.Measure(obj2);
            }
            if (value.C is global::TypeRefs.MsgB.MsgC obj3)
            {
                len += 1 + global::TypeRefs.MsgB.MsgC.Measure(obj3);
            }
            if (value.D is global::TypeRefs.MsgD obj4)
            {
                len += 1 + global::TypeRefs.MsgD.Measure(obj4);
            }
            return len;
        }

        internal static MsgA Merge(MsgA value, ref global::ProtoBuf.Nano.Reader reader)
        {
            ulong oldEnd;
            if (value is null) value = new();
            uint tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: // field 1, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.A = global::TypeRefs.MsgA.Merge(value.A, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 11: // field 1, group
                        value.A = global::TypeRefs.MsgA.Merge(value.A, ref reader);
                        reader.PopGroup(1);
                        break;
                    case 18: // field 2, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.B = global::TypeRefs.MsgB.Merge(value.B, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 19: // field 2, group
                        value.B = global::TypeRefs.MsgB.Merge(value.B, ref reader);
                        reader.PopGroup(2);
                        break;
                    case 26: // field 3, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.C = global::TypeRefs.MsgB.MsgC.Merge(value.C, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 27: // field 3, group
                        value.C = global::TypeRefs.MsgB.MsgC.Merge(value.C, ref reader);
                        reader.PopGroup(3);
                        break;
                    case 34: // field 4, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.D = global::TypeRefs.MsgD.Merge(value.D, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 35: // field 4, group
                        value.D = global::TypeRefs.MsgD.Merge(value.D, ref reader);
                        reader.PopGroup(4);
                        break;
                    default:
                        if ((tag & 7) == 4) // end-group
                        {
                            reader.PushGroup(tag);
                            goto ExitLoop;
                        }
                        switch (tag >> 3)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                reader.UnhandledTag(tag); // throws
                                break;
                        }
                        reader.Skip(tag);
                        break;
                }
            }
        ExitLoop:
            return value;
        }

    }

    [global::ProtoBuf.ProtoContract(Name = @"msg_b")]
    public partial class MsgB : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"a")]
        public global::TypeRefs.MsgA A { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"b")]
        public global::TypeRefs.MsgB B { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"c")]
        public global::TypeRefs.MsgB.MsgC C { get; set; }

        [global::ProtoBuf.ProtoMember(4, Name = @"d")]
        public global::TypeRefs.MsgD D { get; set; }

        [global::ProtoBuf.ProtoContract(Name = @"msg_c")]
        public partial class MsgC : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, Name = @"a")]
            public global::TypeRefs.MsgA A { get; set; }

            [global::ProtoBuf.ProtoMember(2, Name = @"b")]
            public global::TypeRefs.MsgB B { get; set; }

            [global::ProtoBuf.ProtoMember(3, Name = @"c")]
            public global::TypeRefs.MsgB.MsgC C { get; set; }

            [global::ProtoBuf.ProtoMember(4, Name = @"d")]
            public global::TypeRefs.MsgD D { get; set; }

            internal static void Serialize(MsgC value, ref global::ProtoBuf.Nano.Writer writer)
            {
                if (value.A is global::TypeRefs.MsgA obj1)
                {
                    writer.WriteVarint(10); // field 1, string
                    writer.WriteVarintUInt64(global::TypeRefs.MsgA.Measure(obj1);
                    global::TypeRefs.MsgA.Write(obj1, ref writer);
                }
                if (value.B is global::TypeRefs.MsgB obj2)
                {
                    writer.WriteVarint(18); // field 2, string
                    writer.WriteVarintUInt64(global::TypeRefs.MsgB.Measure(obj2);
                    global::TypeRefs.MsgB.Write(obj2, ref writer);
                }
                if (value.C is global::TypeRefs.MsgB.MsgC obj3)
                {
                    writer.WriteVarint(26); // field 3, string
                    writer.WriteVarintUInt64(global::TypeRefs.MsgB.MsgC.Measure(obj3);
                    global::TypeRefs.MsgB.MsgC.Write(obj3, ref writer);
                }
                if (value.D is global::TypeRefs.MsgD obj4)
                {
                    writer.WriteVarint(34); // field 4, string
                    writer.WriteVarintUInt64(global::TypeRefs.MsgD.Measure(obj4);
                    global::TypeRefs.MsgD.Write(obj4, ref writer);
                }
            }

            internal static ulong Measure(MsgC value)
            {
                ulong len = 0;
                if (value.A is global::TypeRefs.MsgA obj1)
                {
                    len += 1 + global::TypeRefs.MsgA.Measure(obj1);
                }
                if (value.B is global::TypeRefs.MsgB obj2)
                {
                    len += 1 + global::TypeRefs.MsgB.Measure(obj2);
                }
                if (value.C is global::TypeRefs.MsgB.MsgC obj3)
                {
                    len += 1 + global::TypeRefs.MsgB.MsgC.Measure(obj3);
                }
                if (value.D is global::TypeRefs.MsgD obj4)
                {
                    len += 1 + global::TypeRefs.MsgD.Measure(obj4);
                }
                return len;
            }

            internal static MsgC Merge(MsgC value, ref global::ProtoBuf.Nano.Reader reader)
            {
                ulong oldEnd;
                if (value is null) value = new();
                uint tag;
                while ((tag = reader.ReadTag()) != 0)
                {
                    switch (tag)
                    {
                        case 10: // field 1, string
                            oldEnd = reader.ConstrainByLengthPrefix();
                            value.A = global::TypeRefs.MsgA.Merge(value.A, ref reader);
                            reader.Unconstrain(oldEnd);
                            break;
                        case 11: // field 1, group
                            value.A = global::TypeRefs.MsgA.Merge(value.A, ref reader);
                            reader.PopGroup(1);
                            break;
                        case 18: // field 2, string
                            oldEnd = reader.ConstrainByLengthPrefix();
                            value.B = global::TypeRefs.MsgB.Merge(value.B, ref reader);
                            reader.Unconstrain(oldEnd);
                            break;
                        case 19: // field 2, group
                            value.B = global::TypeRefs.MsgB.Merge(value.B, ref reader);
                            reader.PopGroup(2);
                            break;
                        case 26: // field 3, string
                            oldEnd = reader.ConstrainByLengthPrefix();
                            value.C = global::TypeRefs.MsgB.MsgC.Merge(value.C, ref reader);
                            reader.Unconstrain(oldEnd);
                            break;
                        case 27: // field 3, group
                            value.C = global::TypeRefs.MsgB.MsgC.Merge(value.C, ref reader);
                            reader.PopGroup(3);
                            break;
                        case 34: // field 4, string
                            oldEnd = reader.ConstrainByLengthPrefix();
                            value.D = global::TypeRefs.MsgD.Merge(value.D, ref reader);
                            reader.Unconstrain(oldEnd);
                            break;
                        case 35: // field 4, group
                            value.D = global::TypeRefs.MsgD.Merge(value.D, ref reader);
                            reader.PopGroup(4);
                            break;
                        default:
                            if ((tag & 7) == 4) // end-group
                            {
                                reader.PushGroup(tag);
                                goto ExitLoop;
                            }
                            switch (tag >> 3)
                            {
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                    reader.UnhandledTag(tag); // throws
                                    break;
                            }
                            reader.Skip(tag);
                            break;
                    }
                }
            ExitLoop:
                return value;
            }

        }

        internal static void Serialize(MsgB value, ref global::ProtoBuf.Nano.Writer writer)
        {
            if (value.A is global::TypeRefs.MsgA obj1)
            {
                writer.WriteVarint(10); // field 1, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgA.Measure(obj1);
                global::TypeRefs.MsgA.Write(obj1, ref writer);
            }
            if (value.B is global::TypeRefs.MsgB obj2)
            {
                writer.WriteVarint(18); // field 2, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgB.Measure(obj2);
                global::TypeRefs.MsgB.Write(obj2, ref writer);
            }
            if (value.C is global::TypeRefs.MsgB.MsgC obj3)
            {
                writer.WriteVarint(26); // field 3, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgB.MsgC.Measure(obj3);
                global::TypeRefs.MsgB.MsgC.Write(obj3, ref writer);
            }
            if (value.D is global::TypeRefs.MsgD obj4)
            {
                writer.WriteVarint(34); // field 4, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgD.Measure(obj4);
                global::TypeRefs.MsgD.Write(obj4, ref writer);
            }
        }

        internal static ulong Measure(MsgB value)
        {
            ulong len = 0;
            if (value.A is global::TypeRefs.MsgA obj1)
            {
                len += 1 + global::TypeRefs.MsgA.Measure(obj1);
            }
            if (value.B is global::TypeRefs.MsgB obj2)
            {
                len += 1 + global::TypeRefs.MsgB.Measure(obj2);
            }
            if (value.C is global::TypeRefs.MsgB.MsgC obj3)
            {
                len += 1 + global::TypeRefs.MsgB.MsgC.Measure(obj3);
            }
            if (value.D is global::TypeRefs.MsgD obj4)
            {
                len += 1 + global::TypeRefs.MsgD.Measure(obj4);
            }
            return len;
        }

        internal static MsgB Merge(MsgB value, ref global::ProtoBuf.Nano.Reader reader)
        {
            ulong oldEnd;
            if (value is null) value = new();
            uint tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: // field 1, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.A = global::TypeRefs.MsgA.Merge(value.A, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 11: // field 1, group
                        value.A = global::TypeRefs.MsgA.Merge(value.A, ref reader);
                        reader.PopGroup(1);
                        break;
                    case 18: // field 2, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.B = global::TypeRefs.MsgB.Merge(value.B, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 19: // field 2, group
                        value.B = global::TypeRefs.MsgB.Merge(value.B, ref reader);
                        reader.PopGroup(2);
                        break;
                    case 26: // field 3, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.C = global::TypeRefs.MsgB.MsgC.Merge(value.C, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 27: // field 3, group
                        value.C = global::TypeRefs.MsgB.MsgC.Merge(value.C, ref reader);
                        reader.PopGroup(3);
                        break;
                    case 34: // field 4, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.D = global::TypeRefs.MsgD.Merge(value.D, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 35: // field 4, group
                        value.D = global::TypeRefs.MsgD.Merge(value.D, ref reader);
                        reader.PopGroup(4);
                        break;
                    default:
                        if ((tag & 7) == 4) // end-group
                        {
                            reader.PushGroup(tag);
                            goto ExitLoop;
                        }
                        switch (tag >> 3)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                reader.UnhandledTag(tag); // throws
                                break;
                        }
                        reader.Skip(tag);
                        break;
                }
            }
        ExitLoop:
            return value;
        }

    }

    [global::ProtoBuf.ProtoContract(Name = @"msg_d")]
    public partial class MsgD : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"a")]
        public global::TypeRefs.MsgA A { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"b")]
        public global::TypeRefs.MsgB B { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"c")]
        public global::TypeRefs.MsgB.MsgC C { get; set; }

        [global::ProtoBuf.ProtoMember(4, Name = @"d")]
        public global::TypeRefs.MsgD D { get; set; }

        internal static void Serialize(MsgD value, ref global::ProtoBuf.Nano.Writer writer)
        {
            if (value.A is global::TypeRefs.MsgA obj1)
            {
                writer.WriteVarint(10); // field 1, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgA.Measure(obj1);
                global::TypeRefs.MsgA.Write(obj1, ref writer);
            }
            if (value.B is global::TypeRefs.MsgB obj2)
            {
                writer.WriteVarint(18); // field 2, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgB.Measure(obj2);
                global::TypeRefs.MsgB.Write(obj2, ref writer);
            }
            if (value.C is global::TypeRefs.MsgB.MsgC obj3)
            {
                writer.WriteVarint(26); // field 3, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgB.MsgC.Measure(obj3);
                global::TypeRefs.MsgB.MsgC.Write(obj3, ref writer);
            }
            if (value.D is global::TypeRefs.MsgD obj4)
            {
                writer.WriteVarint(34); // field 4, string
                writer.WriteVarintUInt64(global::TypeRefs.MsgD.Measure(obj4);
                global::TypeRefs.MsgD.Write(obj4, ref writer);
            }
        }

        internal static ulong Measure(MsgD value)
        {
            ulong len = 0;
            if (value.A is global::TypeRefs.MsgA obj1)
            {
                len += 1 + global::TypeRefs.MsgA.Measure(obj1);
            }
            if (value.B is global::TypeRefs.MsgB obj2)
            {
                len += 1 + global::TypeRefs.MsgB.Measure(obj2);
            }
            if (value.C is global::TypeRefs.MsgB.MsgC obj3)
            {
                len += 1 + global::TypeRefs.MsgB.MsgC.Measure(obj3);
            }
            if (value.D is global::TypeRefs.MsgD obj4)
            {
                len += 1 + global::TypeRefs.MsgD.Measure(obj4);
            }
            return len;
        }

        internal static MsgD Merge(MsgD value, ref global::ProtoBuf.Nano.Reader reader)
        {
            ulong oldEnd;
            if (value is null) value = new();
            uint tag;
            while ((tag = reader.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10: // field 1, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.A = global::TypeRefs.MsgA.Merge(value.A, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 11: // field 1, group
                        value.A = global::TypeRefs.MsgA.Merge(value.A, ref reader);
                        reader.PopGroup(1);
                        break;
                    case 18: // field 2, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.B = global::TypeRefs.MsgB.Merge(value.B, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 19: // field 2, group
                        value.B = global::TypeRefs.MsgB.Merge(value.B, ref reader);
                        reader.PopGroup(2);
                        break;
                    case 26: // field 3, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.C = global::TypeRefs.MsgB.MsgC.Merge(value.C, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 27: // field 3, group
                        value.C = global::TypeRefs.MsgB.MsgC.Merge(value.C, ref reader);
                        reader.PopGroup(3);
                        break;
                    case 34: // field 4, string
                        oldEnd = reader.ConstrainByLengthPrefix();
                        value.D = global::TypeRefs.MsgD.Merge(value.D, ref reader);
                        reader.Unconstrain(oldEnd);
                        break;
                    case 35: // field 4, group
                        value.D = global::TypeRefs.MsgD.Merge(value.D, ref reader);
                        reader.PopGroup(4);
                        break;
                    default:
                        if ((tag & 7) == 4) // end-group
                        {
                            reader.PushGroup(tag);
                            goto ExitLoop;
                        }
                        switch (tag >> 3)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                reader.UnhandledTag(tag); // throws
                                break;
                        }
                        reader.Skip(tag);
                        break;
                }
            }
        ExitLoop:
            return value;
        }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion