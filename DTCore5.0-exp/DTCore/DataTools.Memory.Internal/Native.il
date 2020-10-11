/* ************************************************* *
/* DataTools Visual Basic Utility Library 
/*
/* Module: Internal CIL Code For Global Memory Methods
/* 
/* Copyright (C) 2011-2020 Nathan Moschkin
/* All Rights Reserved
/*
/* Licensed Under the Microsoft Public License   
/* ************************************************* */

.namespace DataTools.Memory.Internal
{
	.class public auto unicode sealed Native
	{
		.method public static void MemSet(native int dest, uint8 'char', uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
            ldarg.1
            ldarg.2

			initblk

			ret
		}


		.method public static void MemCpy(native int dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
            ldarg.1
            ldarg.2

			cpblk

			ret
		}


        // All the MemCpy's!!!!


		.method public static void MemCpy(native int dest, int8[] src, int32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			int8
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(int8[] dest, native int src, int32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			int8
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}
	

    
		.method public static void MemCpy(native int dest, int16[] src, int32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			int16
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(int16[] dest, native int src, int32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			int16
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}
	



		.method public static void MemCpy(native int dest, int32[] src, int32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			int32
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(int32[] dest, native int src, int32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			int32
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}
	



    
		.method public static void MemCpy(native int dest, int64[] src, int32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			int64
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(int64[] dest, native int src, int32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			int64
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}
	


		.method public static void MemCpy(native int dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0         
			ldarg.1         
			ldc.i4.0
			ldelema			uint8
			ldarg.2         

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(uint8[] dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0         
			ldc.i4.0
			ldelema			uint8
			ldarg.1         
			ldarg.2         

			
			cpblk
            

			ret
		}
	
		.method public static void MemCpy(native int dest, char[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			char
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(char[] dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			char
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}

   
		.method public static void MemCpy(native int dest, uint16[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			uint16
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(uint16[] dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint16
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}
	



		.method public static void MemCpy(native int dest, uint32[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			uint32
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(uint32[] dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint32
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}
	



    
		.method public static void MemCpy(native int dest, uint64[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			uint64
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(uint64[] dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint64
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}
	


    
		.method public static void MemCpy(native int dest, float32[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			float32
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(float32[] dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			float32
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}




        .method public static void MemCpy(native int dest, float64[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			float64
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(float64[] dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			float64
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}

        
                

        .method public static void MemCpy(native int dest, valuetype [System.Drawing.Primitives]System.Drawing.Color[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			[System.Drawing.Primitives]System.Drawing.Color
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(valuetype [System.Drawing.Primitives]System.Drawing.Color[] dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			[System.Drawing.Primitives]System.Drawing.Color
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}
        
                

        .method public static void MemCpy(native int dest, valuetype [System.Runtime]System.DateTime[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			[System.Runtime]System.DateTime
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(valuetype [System.Runtime]System.DateTime[] dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			[System.Runtime]System.DateTime
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}
        

        .method public static void MemCpy(native int dest, valuetype [System.Runtime]System.Guid[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			[System.Runtime]System.Guid
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(valuetype [System.Runtime]System.Guid[] dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			[System.Runtime]System.Guid
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}


        .method public static void MemCpy(native int dest, valuetype [System.Runtime]System.Decimal[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldarg.1
			ldc.i4.0
			ldelema			[System.Runtime]System.Decimal
			ldarg.2

			
			cpblk
            
			ret
		}

		.method public static void MemCpy(valuetype [System.Runtime]System.Decimal[] dest, native int src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			[System.Runtime]System.Decimal
			ldarg.1
			ldarg.2

			
			cpblk
            

			ret
		}

        /* End Pointer MemCpy */

        
		.method public static void MemCpy(uint8[] dest, int8[] src, int32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint8
			ldarg.1
			ldc.i4.0
			ldelema			int8
			ldarg.2
		
			cpblk
            
			ret
		}

                
		.method public static void MemCpy(uint8[] dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint8
			ldarg.1
			ldc.i4.0
			ldelema			uint8
			ldarg.2
		
			cpblk
            
			ret
		}
        
		.method public static void MemCpy(uint8[] dest, char[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint8
			ldarg.1
			ldc.i4.0
			ldelema			char
			ldarg.2
		
			cpblk
            
			ret
		}

        
		.method public static void MemCpy(uint8[] dest, int16[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint8
			ldarg.1
			ldc.i4.0
			ldelema			int16
			ldarg.2
		
			cpblk
            
			ret
		}

                
		.method public static void MemCpy(uint8[] dest, uint16[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint8
			ldarg.1
			ldc.i4.0
			ldelema			uint16
			ldarg.2
		
			cpblk
            
			ret
		}

                
		.method public static void MemCpy(uint8[] dest, int32[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint8
			ldarg.1
			ldc.i4.0
			ldelema			int32
			ldarg.2
		
			cpblk
            
			ret
		}

                
		.method public static void MemCpy(uint8[] dest, uint32[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint8
			ldarg.1
			ldc.i4.0
			ldelema			uint32
			ldarg.2
		
			cpblk
            
			ret
		}

        .method public static void MemCpy(uint8[] dest, int64[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint8
			ldarg.1
			ldc.i4.0
			ldelema			int64
			ldarg.2
		
			cpblk
            
			ret
		}

                
		.method public static void MemCpy(uint8[] dest, uint64[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint8
			ldarg.1
			ldc.i4.0
			ldelema			uint64
			ldarg.2
		
			cpblk
            
			ret
		}


        .method public static void MemCpy(uint8[] dest, float32[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint8
			ldarg.1
			ldc.i4.0
			ldelema			float32
			ldarg.2
		
			cpblk
            
			ret
		}

                
		.method public static void MemCpy(uint8[] dest, float64[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint8
			ldarg.1
			ldc.i4.0
			ldelema			float64
			ldarg.2
		
			cpblk
            
			ret
		}

//



        
		.method public static void MemCpy(int8[] dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			int8
			ldarg.1
			ldc.i4.0
			ldelema			uint8
			ldarg.2
		
			cpblk
            
			ret
		}

        .method public static void MemCpy(char[] dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			char
			ldarg.1
			ldc.i4.0
			ldelema			uint8
			ldarg.2
		
			cpblk
            
			ret
		}
        

		.method public static void MemCpy(int16[] dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			int16
			ldarg.1
			ldc.i4.0
			ldelema			uint8
			ldarg.2
		
			cpblk
            
			ret
		}

                
		.method public static void MemCpy(uint16[] dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint16
			ldarg.1
			ldc.i4.0
			ldelema			uint8
			ldarg.2
		
			cpblk
            
			ret
		}

                
		.method public static void MemCpy(int32[] dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			int32
			ldarg.1
			ldc.i4.0
			ldelema			uint8
			ldarg.2
		
			cpblk
            
			ret
		}

                
		.method public static void MemCpy(uint32[] dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint32
			ldarg.1
			ldc.i4.0
			ldelema			uint8
			ldarg.2
		
			cpblk
            
			ret
		}

        .method public static void MemCpy(int64[] dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			int64
			ldarg.1
			ldc.i4.0
			ldelema			uint8
			ldarg.2
		
			cpblk
            
			ret
		}

                
		.method public static void MemCpy(uint64[] dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			uint64
			ldarg.1
			ldc.i4.0
			ldelema			uint8
			ldarg.2
		
			cpblk
            
			ret
		}


        .method public static void MemCpy(float32[] dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			float32
			ldarg.1
			ldc.i4.0
			ldelema			uint8
			ldarg.2
		
			cpblk
            
			ret
		}

                
		.method public static void MemCpy(float64[] dest, uint8[] src, uint32 length) cil managed 
		{
			.maxstack 4

			ldarg.0
			ldc.i4.0
			ldelema			float64
			ldarg.1
			ldc.i4.0
			ldelema			uint8
			ldarg.2
		
			cpblk
            
			ret
		}

                      


        /* End Byte[] (Byte() in VB) MemCpy */

        
        
        /* Begin BitConverter Singletons Byte[] (Byte() in VB) */

        .method public static uint8[] ToBytes(char src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4.2
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4.2
            cpblk
            ldloc.0
			ret
		}

        .method public static char ToChar(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                char    a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4.2
            cpblk

            ldloc.0
            ret
		}


        .method public static uint8[] ToBytes(int16 src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4.2
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4.2
            cpblk
            ldloc.0
			ret
		}

        .method public static int16 ToShort(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                int16   a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4.2
            cpblk

            ldloc.0
            ret
		}


        .method public static uint8[] ToBytes(uint16 src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4.2
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4.2
            cpblk
            ldloc.0
			ret
		}

        .method public static uint16 ToUShort(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint16   a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4.2
            cpblk

            ldloc.0
            ret
		}


        

        .method public static uint8[] ToBytes(int32 src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4.4
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4.4
            cpblk
            ldloc.0
			ret
		}

        .method public static int32 ToInteger(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                int32   a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4.4
            cpblk

            ldloc.0
            ret
		}


        .method public static uint8[] ToBytes(uint32 src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4.4
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4.4
            cpblk
            ldloc.0
			ret
		}

        .method public static uint32 ToUInteger(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint32   a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4.4
            cpblk

            ldloc.0
            ret
		}


        

        .method public static uint8[] ToBytes(int64 src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4.8
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4.8
            cpblk
            ldloc.0
			ret
		}

        .method public static int64 ToLong(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                int64   a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4.8
            cpblk

            ldloc.0
            ret
		}


        .method public static uint8[] ToBytes(uint64 src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4.8
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4.8
            cpblk
            ldloc.0
			ret
		}

        .method public static uint64 ToULong(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint64   a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4.8
            cpblk

            ldloc.0
            ret
		}



        
        .method public static uint8[] ToBytes(float32 src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4.4
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4.4
            cpblk
            ldloc.0
			ret
		}

        .method public static float32 ToSingle(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                float32   a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4.4
            cpblk

            ldloc.0
            ret
		}



        .method public static uint8[] ToBytes(float64 src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4.8
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4.8
            cpblk
            ldloc.0
			ret
		}

        

        .method public static float64 ToDouble(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                float64   a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4.8
            cpblk

            ldloc.0
            ret
		}



        .method public static uint8[] ToBytes(valuetype [System.Runtime]System.DateTime src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4.8
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4.8
            cpblk
            ldloc.0
			ret
		}


        .method public static valuetype [System.Runtime]System.DateTime ToDate(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                valuetype [System.Runtime]System.DateTime a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4.8
            cpblk

            ldloc.0
            ret
		}



        .method public static uint8[] ToBytes(valuetype [System.Runtime]System.Decimal src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4      16
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4      16
            cpblk
            ldloc.0
			ret
		}


        .method public static valuetype [System.Runtime]System.Decimal ToDecimal(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                valuetype [System.Runtime]System.Decimal a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4      16
            cpblk

            ldloc.0
            ret
		}

        
        .method public static uint8[] ToBytes(valuetype [System.Runtime]System.Guid src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a
            )

            ldc.i4      16
            newarr      uint8
            stloc.0
            ldloc.0
            ldc.i4.0
            ldelema     uint8
            ldarga      0
            ldc.i4      16
            cpblk
            ldloc.0
			ret
		}


        .method public static valuetype [System.Runtime]System.Guid ToGuid(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                valuetype [System.Runtime]System.Guid a
            )

            ldloca      0
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add
            ldc.i4      16
            cpblk

            ldloc.0
            ret
		}


                

        /* End BitConverter Singletons Byte[] (Byte() in VB) */



        /* Begin BitConverter Arrays Byte[] (Byte() in VB) */

        .method public static uint8[] ToBytes(char[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.1
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     char
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}

        .method public static char[] ToChars(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                char[]  a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.1
            shr
            ldc.i4.1
            shl
            stloc.1
            ldloc.1
            ldc.i4.1
            shr
            newarr      char
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     char
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}


        .method public static uint8[] ToBytes(int16[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.1
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     int16
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}

        .method public static int16[] ToShorts(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                int16[]  a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.1
            shr
            ldc.i4.1
            shl
            stloc.1
            ldloc.1
            ldc.i4.1
            shr
            newarr      int16
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     int16
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}

        .method public static uint8[] ToBytes(uint16[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.1
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     uint16
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}

        .method public static uint16[] ToUShorts(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint16[]  a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.1
            shr
            ldc.i4.1
            shl
            stloc.1
            ldloc.1
            ldc.i4.1
            shr
            newarr      uint16
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint16
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}




        .method public static uint8[] ToBytes(int32[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.2
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     int32
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}

        .method public static int32[] ToIntegers(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                int32[]  a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.2
            shr
            ldc.i4.2
            shl
            stloc.1
            ldloc.1
            ldc.i4.2
            shr
            newarr      int32
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     int32
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}

        .method public static uint8[] ToBytes(uint32[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.2
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     uint32
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}

        .method public static uint32[] ToUIntegers(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint32[]  a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.2
            shr
            ldc.i4.2
            shl
            stloc.1
            ldloc.1
            ldc.i4.2
            shr
            newarr      uint32
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint32
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}


        

        .method public static uint8[] ToBytes(int64[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.3
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     int64
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}

        .method public static int64[] ToLongs(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                int64[]  a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.3
            shr
            ldc.i4.3
            shl
            stloc.1
            ldloc.1
            ldc.i4.3
            shr
            newarr      int64
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     int64
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}

        .method public static uint8[] ToBytes(uint64[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.3
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     uint64
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}


        .method public static uint64[] ToULongs(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint64[]  a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.3
            shr
            ldc.i4.3
            shl
            stloc.1
            ldloc.1
            ldc.i4.3
            shr
            newarr      uint64
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint64
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}


        .method public static uint8[] ToBytes(float32[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.2
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     float32
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}

        .method public static float32[] ToSingles(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                float32[]  a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.2
            shr
            ldc.i4.2
            shl
            stloc.1
            ldloc.1
            ldc.i4.2
            shr
            newarr      float32
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     float32
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}


        
        .method public static uint8[] ToBytes(float64[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.3
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     float64
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}

        .method public static float64[] ToDoubles(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                float64[]  a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.3
            shr
            ldc.i4.3
            shl
            stloc.1
            ldloc.1
            ldc.i4.3
            shr
            newarr      float64
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     float64
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}


        
        .method public static uint8[] ToBytes(valuetype [System.Runtime]System.DateTime[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.3
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     valuetype [System.Runtime]System.DateTime
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}

        .method public static valuetype [System.Runtime]System.DateTime[] ToDates(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                valuetype [System.Runtime]System.DateTime[]  a,
                int32     len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.3
            shr
            ldc.i4.3
            shl
            stloc.1
            ldloc.1
            ldc.i4.3
            shr
            newarr      valuetype [System.Runtime]System.DateTime
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     valuetype [System.Runtime]System.DateTime
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}





        .method public static uint8[] ToBytes(valuetype [System.Runtime]System.Decimal[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.4
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     valuetype [System.Runtime]System.Decimal
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}

        .method public static valuetype [System.Runtime]System.Decimal[] ToDecimals(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                valuetype [System.Runtime]System.Decimal[]  a,
                int32     len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.4
            shr
            ldc.i4.4
            shl
            stloc.1
            ldloc.1
            ldc.i4.4
            shr
            newarr      valuetype [System.Runtime]System.Decimal
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     valuetype [System.Runtime]System.Decimal
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}



        .method public static uint8[] ToBytes(valuetype [System.Runtime]System.Guid[] src) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                uint8[] a,
                int32   len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldc.i4.4
            shl
            stloc.1
            ldloc.1
            newarr      uint8
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     uint8
            
            ldarg.0
            ldc.i4.0
            ldelema     valuetype [System.Runtime]System.Guid
            
            ldloc.1
            cpblk

            ldloc.0
            ret
		}

        .method public static valuetype [System.Runtime]System.Guid[] ToGuids(uint8[] src, uint32 index) cil managed 
		{
			.maxstack 4
            .locals init 
            (
                valuetype [System.Runtime]System.Guid[]  a,
                int32     len
            )

            ldarg.0
            call        instance int32 [System.Runtime]System.Array::get_Length()
            ldarg.1
            sub
            ldc.i4.4
            shr
            ldc.i4.4
            shl
            stloc.1
            ldloc.1
            ldc.i4.4
            shr
            newarr      valuetype [System.Runtime]System.Guid
            stloc.0

            ldloc.0
            ldc.i4.0
            ldelema     valuetype [System.Runtime]System.Guid
            
            ldarg.0
            ldc.i4.0
            ldelema     uint8
            ldarg.1
            conv.i
            add

            ldloc.1
            cpblk

            ldloc.0
            ret

		}






        /* End BitConverter Arrays Byte[] (Byte() in VB) */



        /* Begin Bit-Wise Verbatim in VB) */

		.method public static native int CIntPtr(int32 'value') cil managed 
		{
			.maxstack 4

			ldarg.s         'value'
			conv.i
			ret
		}

		.method public static native int CIntPtr(int64 'value') cil managed 
		{
			.maxstack 4

			ldarg.s         'value'
			conv.i
			ret
		}
		

        
		.method public static int32 PtrTo32(native int 'value') cil managed 
		{
			.maxstack 4

			ldarg.s         'value'
			conv.i4
			ret
		}
		
            
		.method public static int64 PtrTo64(native int 'value') cil managed 
		{
			.maxstack 4

			ldarg.s         'value'
			conv.i8
			ret
		}
		

		.method public static native uint ToUnsigned(native int val) cil managed 
		{
			ldarga.s 0
			ldind.i
		
			ret
		}

		.method public static native int ToSigned(native uint val) cil managed 
		{
			ldarga.s 0
			ldind.i
	
			ret
		}

		.method public static uint8 ToUnsigned(int8 val) cil managed 
		{
			ldarga.s 0
			ldind.u1
	
			ret
		}

		.method public static int8 ToSigned(uint8 val) cil managed 
		{
			ldarga.s 0
			ldind.i1

			ret
		}


		.method public static uint16 ToUnsigned(int16 val) cil managed 
		{
			ldarga.s 0
			ldind.u2

			ret
		}

		.method public static int16 ToSigned(uint16 val) cil managed 
		{
			ldarga.s 0
			ldind.i2

			ret
		}


		.method public static uint32 ToUnsigned(int32 val) cil managed 
		{
			ldarga.s 0
			ldind.u4

			ret
		}

		.method public static int32 ToSigned(uint32 val) cil managed 
		{
			ldarga.s 0
			ldind.i4

			ret
		}

		.method public static uint64 ToUnsigned(int64 val) cil managed 
		{
			ldarga.s 0
			ldind.u8

			ret
		}

		.method public static int64 ToSigned(uint64 val) cil managed 
		{
			ldarga.s 0
			ldind.i8

			ret
		}

        /* End Bit-Wise Verbatim in VB) */

        
		// Get a null-terminated Unicode string length from a pointer.
        .method public static int32 CharZero(native int ptr) cil managed 
        {
            .maxstack	3
            .locals init 
            (
                native int pinc
            )
    
            ldarg.s     ptr
       		stloc.s     pinc

    SCANLOOP:

            ldloc.s     pinc
			ldind.u2
            ldc.i4.0

            beq			FINISHER

            ldloc.s     pinc
            ldc.i4.2
            conv.i
            add
            stloc.s     pinc

            br			SCANLOOP

    FINISHER:

			ldloc.s     pinc
			ldarg.s     ptr
            sub
            conv.i4
			ldc.i4.1
			shr
            ret
        }


   		// Get a null-terminated UTF-8 or Ascii string length from a pointer.
        .method public static int32 ByteZero(native int ptr) cil managed 
        {
            .maxstack	3
            .locals init 
            (
                native int pinc
            )
    
            ldarg.s     ptr
       		stloc.s     pinc

    SCANLOOP:

            ldloc.s     pinc
			ldind.u1
            ldc.i4.0

            beq			FINISHER

            ldloc.s     pinc
            ldc.i4.1
            conv.i
            add
            stloc.s     pinc

            br			SCANLOOP

    FINISHER:

			ldloc.s     pinc
			ldarg.s     ptr
            sub
            conv.i4
            ret
        }


	}

}