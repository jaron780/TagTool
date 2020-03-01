-- Author: Beatthezombie 
-- License: GNU General Public License v3.0 http://www.gnu.org/licenses/gpl.txt

--
-- Halo Geometry Format Definitions
--

global string_length = 0x40


--
-- Helper functions to parse the file
--

fn ReadFixedLengthString stream =
(
    position = ftell stream
    str = ReadString stream
    new_position = position + string_length
    fseek stream new_position #seek_set
    str
)

struct tag_block
(
    count = 0,
    offset = 0,

    fn read stream =
    (
        count = ReadLong stream
        offset = ReadLong stream
        ReadLong stream
    ),

    fn write stream =
    (
        WriteLong stream count
        WriteLong stream offset
        WriteLong stream 0
    )
)

struct halo_geometry_format_header
(
    signature = 0,
    version = 0,
    flags = 0,
    content_flags = 0,
	offset = 0x14,

    fn read f = 
    (
        signature = ReadLong f #unsigned
        version = ReadLong f 
        flags = ReadLong f 
        content_flags = ReadLong f
		offset = ReadLong f
    ),

    fn write f =
    (
        WriteLong f signature #unsigned
        WriteLong f version
        WriteLong f flags
        WriteLong f content_flags
		WriteLong f offset
    )
)

struct halo_material
(
	name = "",
	
	fn read stream = 
	(
		name = ReadFixedLengthString stream
	),
	
	fn write stream =
	(
		
	)
)

struct halo_geometry_format
(
	header = halo_geometry_format_header(),
	name = "",
    materials =#(),
    nodes = #(),
    meshes = #(),
    sh_order_3_red = #(),
    sh_order_3_green = #(),
    sh_order_3_blue = #(),
    light_probes = tag_block(),
    bounding_spheres = tag_block(),

    fn read stream =
    (
		header.read(stream)
		
		-- seek to the begininig of the geometry struct
		fseek stream header.offset #seek_set
		
		name = ReadFixedLengthString stream
		
		material_block = tag_block()
		nodes_block = tag_block()
		meshes_block = tag_block()
		
        material_block.read(stream)
        nodes_block.read(stream)
        meshes_block.read(stream)

        for i = 1 to 16 do sh_order_3_red[i] = ReadFloat stream
        for i = 1 to 16 do sh_order_3_green[i] = ReadFloat stream
        for i = 1 to 16 do sh_order_3_blue[i] = ReadFloat stream
        light_probes.read(stream)
        bounding_spheres.read(stream)
		
		fseek stream material_block.offset #seek_set

		for i = 1 to material_block.count do
		(
			geometry_material = halo_material()
			geometry_material.read(stream)
			new_material = StandardMaterial()
			new_material.name = geometry_material.name
			materials[i] = new_material
		)
    )

)
